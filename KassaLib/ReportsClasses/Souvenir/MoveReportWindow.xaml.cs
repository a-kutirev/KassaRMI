using KassaLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using Telerik.Reporting;
using Telerik.Windows.Controls;

namespace KassaLib.ReportsClasses.Souvenir
{
    /// <summary>
    /// Interaction logic for MoveReportWindow.xaml
    /// </summary>
    public partial class MoveReportWindow
    {
        #region Members
        #endregion

        #region Ctor
        public MoveReportWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            ReportDate.SelectedValue = Option.CurrentDate;

            ReportDateFrom.SelectedValue = Option.CurrentDate;
            ReportDateTo.SelectedValue = Option.CurrentDate;
        }
        #endregion

        #region Events
        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            string sql_prihod = "";
            string sql_sell = "";
            string sql_spis = "";
            string header = "";

            if ((bool)rb1.IsChecked)
            {
                string date = ((DateTime)ReportDate.SelectedValue).ToString("yyyy-MM-dd");

                sql_prihod =
                            $"SELECT t.idnomenclatura, sum(t.prihod_amount) as amount FROM " +
                            $"(select * from prihod where date_ = '{date}') t " +
                            $"group by t.idnomenclatura ";
                sql_sell =
                            $"SELECT t.idnomenclatura, sum(t.amount) as amount FROM " +
                            $"(select * from sell where date_ = '{date}') t " +
                            $"group by t.idnomenclatura";
                sql_spis =
                            $"SELECT t.idnomenclatura, sum(t.amount) as amount FROM " +
                            $"(select * from sell where date_ = '{date}' and spisanie = 1) t " +
                            $"group by t.idnomenclatura ";

                header = $"Движение товара за {((DateTime)ReportDate.SelectedValue).ToString("dd MMMM yyyy")} г.";
            }
            else
            {
                string datefrom = ((DateTime)ReportDateFrom.SelectedValue).ToString("yyyy-MM-dd");
                string dateto   = ((DateTime)ReportDateTo.SelectedValue).ToString("yyyy-MM-dd");

                sql_prihod =
                            $"SELECT t.idnomenclatura, sum(t.prihod_amount) as amount FROM " +
                            $"(select * from prihod where date_ between '{datefrom}' and '{dateto}') t " +
                            $"group by t.idnomenclatura ";
                sql_sell =
                            $"SELECT t.idnomenclatura, sum(t.amount) as amount FROM " +
                            $"(select * from sell where date_ between '{datefrom}' and '{dateto}') t " +
                            $"group by t.idnomenclatura";

                sql_spis =
                            $"SELECT t.idnomenclatura, sum(t.amount) as amount FROM " +
                            $"(select * from sell where spisanie = 1 and date_ between '{datefrom}' and '{dateto}') t " +
                            $"group by t.idnomenclatura";

                ReportDate.SelectedValue = ReportDateFrom.SelectedValue;
                header = $"Движение товара за период с " +
                    $"{((DateTime)ReportDateFrom.SelectedValue).ToString("dd MMMM yyyy")} г. по " +
                    $"{((DateTime)ReportDateTo.SelectedValue).ToString("dd MMMM yyyy")} г.";
            }

            DataTable prihod = DBWrapper.Select(sql_prihod);
            DataTable sell = DBWrapper.Select(sql_sell);
            DataTable spis = DBWrapper.Select(sql_spis);

            SortedDictionary<int, MoveSouvClass> moveHelper = new SortedDictionary<int, MoveSouvClass>();

            Dictionary<int, int> spisanieDIct = new Dictionary<int, int>();

            for(int i = 0; i < spis.Rows.Count; i++)
            {
                int id = int.Parse(spis.Rows[i]["idnomenclatura"].ToString());
                if(!spisanieDIct.ContainsKey(id))
                {                    
                    int sp = int.Parse(spis.Rows[i]["amount"].ToString());
                    spisanieDIct.Add(id,sp);
                }
            }

            for (int i = 0; i < prihod.Rows.Count; i++)
            {
                int id = int.Parse(prihod.Rows[i]["idnomenclatura"].ToString());
                if (!moveHelper.ContainsKey(id))
                {
                    NomenklaturaModel m = new NomenklaturaModel(id);
                    MoveSouvClass msc = new MoveSouvClass(m.Nomenklaturaname);
                    msc.idNom = m.Idnomenklatura;
                    moveHelper.Add(id, msc);
                }

                moveHelper[id].prihod = int.Parse(prihod.Rows[i]["amount"].ToString());
            }
            for (int i = 0; i < sell.Rows.Count; i++)
            {
                int id = int.Parse(sell.Rows[i]["idnomenclatura"].ToString());
                if (!moveHelper.ContainsKey(id))
                {
                    NomenklaturaModel m = new NomenklaturaModel(id);
                    MoveSouvClass msc = new MoveSouvClass(m.Nomenklaturaname);
                    msc.idNom = m.Idnomenklatura;
                    moveHelper.Add(id, msc);
                }

                moveHelper[id].sell = int.Parse(sell.Rows[i]["amount"].ToString());
            }

            Dictionary<int, int> dt = Option.CalculateIdBalanceOnDate((DateTime)ReportDate.SelectedValue, true);
           
            for(int i = 0; i < moveHelper.Count; i++)
            {
                int id = moveHelper.ElementAt(i).Key;

                if(dt.ContainsKey(id))
                {
                    moveHelper[id].bal_start = dt[id];                    
                }

                moveHelper[id].bal_end = moveHelper[id].bal_start + moveHelper[id].prihod - moveHelper[id].sell;
            }

            List<MoveSouvClass> sortedMoveHelper = new List<MoveSouvClass>();
            List<int> usedIdNom = new List<int>();
            
            for (int i = 0; i < moveHelper.Count; i++)
            {
                MoveSouvClass msc = moveHelper.ElementAt(i).Value;
                usedIdNom.Add(msc.idNom);
                sortedMoveHelper.Add(msc);
            }

            DataTable allNom = Option.CalculateBalanceOnDateExt((DateTime)ReportDateFrom.SelectedValue, false);

            for (int i = 0; i < allNom.Rows.Count; i++)
            {
                int id = (int)allNom.Rows[i]["id"];
                if (usedIdNom.Contains(id)) continue;
                if (((int)allNom.Rows[i]["balance"]) == 0) continue;

                NomenklaturaModel nm = new NomenklaturaModel(id);
                MoveSouvClass msc = new MoveSouvClass(nm.Nomenklaturaname);
                msc.bal_start = (int)allNom.Rows[i]["balance"];
                msc.bal_end = msc.bal_start;
                msc.prihod = 0;

                sortedMoveHelper.Add(msc);
            }

            sortedMoveHelper.Sort();

            DataTable tmp = new DataTable();
            tmp.Columns.Add("name");
            tmp.Columns.Add("bal_start");
            tmp.Columns.Add("prihod");
            tmp.Columns.Add("sell");
            tmp.Columns.Add("bal_end");


            for(int i = 0; i < sortedMoveHelper.Count; i++)
            {
                DataRow dr = tmp.NewRow();
                dr["name"] = sortedMoveHelper[i].Name;
                dr["bal_start"] = sortedMoveHelper[i].bal_start;
                dr["prihod"] = sortedMoveHelper[i].prihod;

                int id = sortedMoveHelper[i].idNom;

                if(spisanieDIct.ContainsKey(id))
                    dr["sell"] = $"{sortedMoveHelper[i].sell - spisanieDIct[id]} / {spisanieDIct[id]}";
                else
                    dr["sell"] = $"{sortedMoveHelper[i].sell} / -";

                dr["bal_end"] = sortedMoveHelper[i].bal_end;
                tmp.Rows.Add(dr);
            }

            string json = JsonConvert.SerializeObject(tmp);

            UriReportSource uriReportSource = new UriReportSource();
            uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//MoveSouvenir.trdp";
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("header", header));
            report.ReportSource = uriReportSource;
        }
        #endregion
    }

    public class MoveSouvClass : IComparer<MoveSouvClass>, IComparable
    {
        public string Name { get; set; } = "";
        public int idNom { get; set; } = -1;
        public int bal_start { get; set; } = 0;
        public int bal_end { get; set; } = 0;
        public int prihod { get; set; } = 0;
        public int sell { get; set; } = 0;

        public MoveSouvClass(string name)
        {
            Name = name;
        }

        public int Compare(MoveSouvClass x, MoveSouvClass y)
        {
            return string.Compare(x.Name, y.Name);
        }

        public int CompareTo(object obj)
        {
            if (Name == null) return 0;

            if (obj is MoveSouvClass person) return Name.CompareTo(person.Name);
            else throw new ArgumentException("Некорректное значение параметра");
        }
    }
}
