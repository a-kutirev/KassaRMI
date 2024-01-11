using KassaLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Reporting;
using CheckBox = System.Windows.Controls.CheckBox;
using TextBox = System.Windows.Controls.TextBox;

namespace KassaLib.ReportsClasses.Souvenir
{
    /// <summary>
    /// Interaction logic for MoveConfigReportWindow.xaml
    /// </summary>
    public partial class MoveConfigReportWindow
    {
        #region Members
        List<CheckBox> checked_list = new List<CheckBox>();
        #endregion

        #region Ctor
        public MoveConfigReportWindow()
        {
            InitializeComponent();

            ReportDateFrom.SelectedDate = Option.CurrentDate;
            ReportDateTo.SelectedDate = Option.CurrentDate;

            string sql =
                        "SELECT nomenklatura.idnomenklatura, nomenklatura.nomenklaturaname,price.price " +
                        "FROM kassa.nomenklatura " +
                        "inner join price on nomenklatura.idnomenklatura = price.idnomenclatura ";

            DataTable tmp = DBWrapper.Select(sql);

            for (int i = 0; i < tmp.Rows.Count; i++)
            {
                int id = int.Parse(tmp.Rows[i]["idnomenklatura"].ToString());
                string n = tmp.Rows[i]["nomenklaturaname"].ToString();

                CheckBox chk = new CheckBox();
                chk.Content = n;
                chk.CommandParameter = id;

                checked_list.Add(chk);
                NomListBox.Items.Add(chk);
            }
        }
        #endregion

        #region Events
        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt_from = ((DateTime)ReportDateFrom.SelectedValue).Date;
            DateTime dt_end = ((DateTime)ReportDateTo.SelectedValue).Date;
            string datefrom = ((DateTime)ReportDateFrom.SelectedValue).ToString("yyyy-MM-dd");
            string dateto = ((DateTime)ReportDateTo.SelectedValue).ToString("yyyy-MM-dd");

            string list = "";
            List<int> sel = new List<int>();

            for (int i = 0; i < checked_list.Count; i++)
            {
                int id = (int)checked_list[i].CommandParameter;
                if ((bool)checked_list[i].IsChecked)
                {
                    sel.Add(id);
                    list += list == "" ? $"{id}" : $", {id}";
                }
            }

            string sql_prihod =
                $"select date(t.date_) as dt, t.idnomenclatura as id, sum(t.prihod_amount) as amount from " +
                $"(SELECT * FROM kassa.prihod where date(date_) between '{datefrom}' and '{dateto}'  " +
                $"and idnomenclatura in ({list})) t " +
                $"group by date(t.date_), t.idnomenclatura order by date(t.date_) asc";

            string sql_sell =
                $"select date(t.date_) as dt, t.idnomenclatura as id, sum(t.amount) as amount from " +
                $"(SELECT * FROM sell where date(date_) between '{datefrom}' and '{dateto}'  " +
                $"and idnomenclatura in ({list})) t " +
                $"group by date(t.date_), t.idnomenclatura  order by date(t.date_) asc";

            DataTable dt_prih = DBWrapper.Select(sql_prihod);
            DataTable dt_sell = DBWrapper.Select(sql_sell);

            List<HelperClass> helperList = new List<HelperClass>();

            for(int i = 0; i < dt_prih.Rows.Count; i++)
            {
                HelperClass hp = new HelperClass();
                hp.date = Convert.ToDateTime(dt_prih.Rows[i]["dt"].ToString());
                hp.amount = int.Parse(dt_prih.Rows[i]["amount"].ToString());
                hp.id = int.Parse(dt_prih.Rows[i]["id"].ToString());
                hp.operation = OperationType.prih;
                helperList.Add(hp);
            }

            for (int i = 0; i < dt_sell.Rows.Count; i++)
            {
                HelperClass hp = new HelperClass();
                hp.date = Convert.ToDateTime(dt_sell.Rows[i]["dt"].ToString()).Date;
                hp.amount = int.Parse(dt_sell.Rows[i]["amount"].ToString());
                hp.id = int.Parse(dt_sell.Rows[i]["id"].ToString());
                hp.operation = OperationType.prod;
                helperList.Add(hp);
            }

            SortedDictionary<DateTime, SortedDictionary<int, MoveSouvClass>> helperDict =
                new SortedDictionary<DateTime, SortedDictionary<int, MoveSouvClass>>();

            for(DateTime tmp = dt_from; tmp <= dt_end; tmp = tmp.AddDays(1))
                helperDict.Add(tmp, null);
            DataTable allNom = Option.CalculateBalanceOnDateExt(dt_from, true);

            // Начальный баланс
            for (int i = 0; i < allNom.Rows.Count; i++)
            {
                int id = Convert.ToInt32(allNom.Rows[i]["id"]);        
                if(sel.Contains(id))
                {
                    if(helperDict[dt_from] == null)
                        helperDict[dt_from] = new SortedDictionary<int, MoveSouvClass>();
                    MoveSouvClass msc = new MoveSouvClass((new NomenklaturaModel(id)).Nomenklaturaname);
                    msc.bal_start = Convert.ToInt32(allNom.Rows[i]["balance"]);
                    msc.idNom = id;

                    helperDict[dt_from].Add(id, msc);
                }
            }
            // Проверяем и добавляем новые позиции

            for(int i = 0; i < sel.Count; i++)
            {
                if (helperDict[dt_from] == null)
                    helperDict[dt_from] = new SortedDictionary<int, MoveSouvClass>();

                if (helperDict[dt_from].ContainsKey(sel[i])) continue;

                MoveSouvClass msc = new MoveSouvClass((new NomenklaturaModel(sel[i])).Nomenklaturaname);
                msc.idNom = sel[i];

                helperDict[dt_from].Add(sel[i], msc);
            }

            // Приход и продажи

            for (int i = 0; i < helperList.Count; i++)
            {
                HelperClass hp = helperList[i];
                if (helperDict[hp.date] == null)
                    helperDict[hp.date] = new SortedDictionary<int, MoveSouvClass>();

                if (!helperDict[hp.date].ContainsKey(hp.id))
                {
                    helperDict[hp.date].Add(hp.id, new MoveSouvClass((new NomenklaturaModel(hp.id)).Nomenklaturaname));
                    helperDict[hp.date][hp.id].idNom = hp.id;
                }

                switch(hp.operation)
                {
                    case OperationType.prih:
                        helperDict[hp.date][hp.id].prihod = hp.amount;
                        break;
                    case OperationType.prod:
                        helperDict[hp.date][hp.id].sell = hp.amount;
                        break;
                }
            }

            int counter = helperDict.Count;
            for(int i = (counter - 2); i >= 0; i--)
            {
                if (helperDict.ElementAt(i).Value == null)
                    helperDict.Remove(helperDict.ElementAt(i).Key);
            }

            Dictionary<int, int> id_bal = new Dictionary<int, int>();
            for (int i = 0; i < sel.Count; i++) 
                id_bal.Add(sel[i], helperDict[dt_from][sel[i]].bal_start);

            for (int i = 0; i < sel.Count; i++)
            {
                if (helperDict[dt_end] == null)
                {
                    helperDict[dt_end] = new SortedDictionary<int, MoveSouvClass>();

                    for (int j = 0; j < sel.Count; j++)
                    {
                        helperDict[dt_end].Add(sel[j], new MoveSouvClass((new NomenklaturaModel(sel[j])).Nomenklaturaname));
                        helperDict[dt_end][sel[j]].idNom = sel[j];
                    }
                }
            }


            for(int i = 0; i < helperDict.Count; i++)
            {
                int c = helperDict[helperDict.ElementAt(i).Key].Count;
                for(int j = 0; j < c; j++)
                {
                    DateTime dateKey = helperDict.ElementAt(i).Key;
                    MoveSouvClass msc = helperDict[dateKey][helperDict[dateKey].ElementAt(j).Key];

                    int id = msc.idNom;
                    msc.bal_start = id_bal[id];
                    msc.bal_end = msc.bal_start + msc.prihod - msc.sell;
                    id_bal[id] = msc.bal_end;
                }
            }

            DataTable res = new DataTable();
            res.Columns.Add("data", typeof(string));
            res.Columns.Add("id", typeof(int));
            res.Columns.Add("name", typeof(string));
            res.Columns.Add("balance_start", typeof(int));
            res.Columns.Add("prihod", typeof(int));
            res.Columns.Add("sell", typeof(int));
            res.Columns.Add("balance_end", typeof(int));

            for (int i = 0; i < helperDict.Count; i++)
            {
                DateTime dateKey = helperDict.ElementAt(i).Key;

                for(int j = 0; j < helperDict[dateKey].Count; j++)
                {
                    DataRow row = res.NewRow();

                    MoveSouvClass msc = helperDict[dateKey].ElementAt(j).Value;
                    row["id"] = msc.idNom;
                    row["data"] = dateKey.ToString("dd MMMM yyyy");
                    row["name"] = msc.Name;
                    row["balance_start"] = msc.bal_start;
                    row["prihod"] = msc.prihod;
                    row["sell"] = msc.sell;
                    row["balance_end"] = msc.bal_end;
                    res.Rows.Add(row);
                }
            }

            int i1 = 0;


                string json = JsonConvert.SerializeObject(res);
                string header = $"Движение товара за период с {datefrom} по {dateto}";

                UriReportSource uriReportSource = new UriReportSource();
                uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//SelectedTovarMove.trdp";
                uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
                uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("header", header));
                report.ReportSource = uriReportSource;
            }

        #region Старый вариант
        //private void ShowReport_Click(object sender, RoutedEventArgs e)
        //{
        //    string datefrom = ((DateTime)ReportDateFrom.SelectedValue).ToString("yyyy-MM-dd");
        //    string dateto = ((DateTime)ReportDateTo.SelectedValue).ToString("yyyy-MM-dd");

        //    string list = "";

        //    for (int i = 0; i < checked_list.Count; i++)
        //    {
        //        int id = (int)checked_list[i].CommandParameter;
        //        if ((bool)checked_list[i].IsChecked)
        //            list += list == "" ? $"{id}" : $", {id}";
        //    }

        //    string sql_prihod =
        //                $"SELECT t.idnomenclatura, sum(t.prihod_amount) as amount FROM " +
        //                $"(select * from prihod where (date_ between '{datefrom}' and '{dateto}') and idnomenclatura in ({list})) t " +
        //                $"group by t.idnomenclatura ";
        //    string sql_sell =
        //                $"SELECT t.idnomenclatura, sum(t.amount) as amount FROM " +
        //                $"(select * from sell where (date_ between '{datefrom}' and '{dateto}') and idnomenclatura in ({list})) t " +
        //                $"group by t.idnomenclatura";
        //    string sql_spis =
        //                $"SELECT t.idnomenclatura, sum(t.amount) as amount FROM " +
        //                $"(select * from sell where spisanie = 1 and (date_ between '{datefrom}' and '{dateto}')  and idnomenclatura in ({list})) t " +
        //                $"group by t.idnomenclatura";

        //    DataTable prihod = DBWrapper.Select(sql_prihod);
        //    DataTable sell = DBWrapper.Select(sql_sell);
        //    DataTable spis = DBWrapper.Select(sql_spis);

        //    SortedDictionary<int, MoveSouvClass> moveHelper = new SortedDictionary<int, MoveSouvClass>();

        //    Dictionary<int, int> spisanieDIct = new Dictionary<int, int>();

        //    for (int i = 0; i < spis.Rows.Count; i++)
        //    {
        //        int id = int.Parse(spis.Rows[i]["idnomenclatura"].ToString());
        //        if (!spisanieDIct.ContainsKey(id))
        //        {
        //            int sp = int.Parse(spis.Rows[i]["amount"].ToString());
        //            spisanieDIct.Add(id, sp);
        //        }
        //    }

        //    for (int i = 0; i < prihod.Rows.Count; i++)
        //    {
        //        int id = int.Parse(prihod.Rows[i]["idnomenclatura"].ToString());
        //        if (!moveHelper.ContainsKey(id))
        //        {
        //            NomenklaturaModel m = new NomenklaturaModel(id);
        //            moveHelper.Add(id, new MoveSouvClass(m.Nomenklaturaname));
        //        }

        //        moveHelper[id].prihod = int.Parse(prihod.Rows[i]["amount"].ToString());
        //    }
        //    for (int i = 0; i < sell.Rows.Count; i++)
        //    {
        //        int id = int.Parse(sell.Rows[i]["idnomenclatura"].ToString());
        //        if (!moveHelper.ContainsKey(id))
        //        {
        //            NomenklaturaModel m = new NomenklaturaModel(id);
        //            moveHelper.Add(id, new MoveSouvClass(m.Nomenklaturaname));
        //        }

        //        moveHelper[id].sell = int.Parse(sell.Rows[i]["amount"].ToString());
        //    }

        //    Dictionary<int, int> dt = Option.CalculateIdBalanceOnDate((DateTime)ReportDateFrom.SelectedValue, true);

        //    for (int i = 0; i < moveHelper.Count; i++)
        //    {
        //        int id = moveHelper.ElementAt(i).Key;

        //        if (dt.ContainsKey(id))
        //        {
        //            moveHelper[id].bal_start = dt[id];
        //        }

        //        moveHelper[id].bal_end = moveHelper[id].bal_start + moveHelper[id].prihod - moveHelper[id].sell;
        //    }

        //    DataTable tmp = new DataTable();
        //    tmp.Columns.Add("name");
        //    tmp.Columns.Add("bal_start");
        //    tmp.Columns.Add("prihod");
        //    tmp.Columns.Add("sell");
        //    tmp.Columns.Add("bal_end");


        //    for (int i = 0; i < moveHelper.Count; i++)
        //    {
        //        DataRow dr = tmp.NewRow();
        //        dr["name"] = moveHelper.ElementAt(i).Value.Name;
        //        dr["bal_start"] = moveHelper.ElementAt(i).Value.bal_start;
        //        dr["prihod"] = moveHelper.ElementAt(i).Value.prihod;

        //        int id = moveHelper.ElementAt(i).Key;
        //        if (spisanieDIct.ContainsKey(id))
        //            dr["sell"] = $"{moveHelper.ElementAt(i).Value.sell - spisanieDIct[id]} / {spisanieDIct[id]}";
        //        else
        //            dr["sell"] = $"{moveHelper.ElementAt(i).Value.sell} / -";
        //        dr["bal_end"] = moveHelper.ElementAt(i).Value.bal_end;
        //        tmp.Rows.Add(dr);
        //    }

        //    string json = JsonConvert.SerializeObject(tmp);
        //    string header = "";


        //    UriReportSource uriReportSource = new UriReportSource();
        //    uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//MoveSouvenir.trdp";
        //    uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
        //    uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("header", header));
        //    report.ReportSource = uriReportSource;
        //}
        #endregion

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string t = (sender as TextBox).Text;
            NomListBox.Items.Clear();
            for (int i = 0; i < checked_list.Count; i++)
            {
                string cb = checked_list[i].Content.ToString().ToUpper();

                if (cb.Contains(t.ToUpper()))
                    NomListBox.Items.Add(checked_list[i]);
            }
        }
        #endregion
    }

    public struct HelperClass
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public int amount { get; set; }        
        public OperationType operation { get; set; }
    }

    public enum OperationType
    {
        prod, prih
    }
}
