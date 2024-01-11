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
    /// Interaction logic for SellSouvReportWindow.xaml
    /// </summary>
    public partial class SellSouvReportWindow
    {
        #region Members
        #endregion

        #region Ctor
        public SellSouvReportWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            ReportDate.SelectedValue = DateTime.Now;
            ReportDateFrom.SelectedValue = DateTime.Now;
            ReportDateTo.SelectedValue = DateTime.Now;
        }
        #endregion

        #region Events
        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            string sql = "";
            string type = "";
            string header = "";

            if ((bool)rb1.IsChecked)
            {
                string t1 = ((DateTime)ReportDate.SelectedValue).ToString("yyyy-MM-dd");

                sql =
                     $"select t.idnomenclatura, t.cash_card, sum(t.amount) as amount, sum(t.price_total) as price from " +
                     $"(select * from sell where date_ = '{t1}' and spisanie = 0) t " +
                     $"group by t.idnomenclatura, t.cash_card";

                type = "day";
                header = $"Продажи товара за {((DateTime)ReportDate.SelectedValue).ToString("dd MMMM yyyy")} г.";
            }
            if ((bool)rb2.IsChecked)
            {
                string t1 = ((DateTime)ReportDateFrom.SelectedValue).ToString("yyyy-MM-dd");
                string t2 = ((DateTime)ReportDateTo.SelectedValue).ToString("yyyy-MM-dd");

                sql =
                     $"select t.idnomenclatura, t.cash_card, sum(t.amount) as amount, sum(t.price_total) as price from " +
                     $"(select * from sell where date_ between '{t1}' and '{t2}' and spisanie = 0) t " +
                     $"group by t.idnomenclatura, t.cash_card";

                type = "period";
                header = $"Продажи товара за период с " +
                    $"{((DateTime)ReportDateFrom.SelectedValue).ToString("dd MMMM yyyy")} г. по " +
                    $"{((DateTime)ReportDateTo.SelectedValue).ToString("dd MMMM yyyy")} г.";
            }

            DataTable tmp = DBWrapper.Select(sql);
            //  nomenkl_id -> sellrecord
            Dictionary<int, SellRecord> records = new Dictionary<int, SellRecord>();

            for(int i = 0; i < tmp.Rows.Count; i++)
            {
                int id_n = (int)tmp.Rows[i]["idnomenclatura"];
                string c_c = tmp.Rows[i]["cash_card"].ToString();

                if(!records.ContainsKey(id_n))
                {
                    NomenklaturaModel model = new NomenklaturaModel(id_n);
                    records.Add(id_n, new SellRecord(model.Nomenklaturaname));
                }

                int amount = int.Parse(tmp.Rows[i]["amount"].ToString());
                float price = float.Parse(tmp.Rows[i]["price"].ToString());

                if (c_c == "cash")
                {
                    records[id_n].cash_amount = amount;
                    records[id_n].cash_total_price = price;
                }
                if(c_c == "card")
                {
                    records[id_n].card_amount = amount;
                    records[id_n].card_total_price = price;
                }
            }

            DataTable rep = new DataTable();
            rep.Columns.Add("name", typeof(string));
            rep.Columns.Add("cash_amount", typeof(string));
            rep.Columns.Add("card_amount", typeof(string));
            rep.Columns.Add("total_amount", typeof(string));
            rep.Columns.Add("cash_price", typeof(string));
            rep.Columns.Add("card_price", typeof(string));
            rep.Columns.Add("total_price", typeof(string));

            for(int i = 0; i < records.Count; i++)
            {
                SellRecord sr = records.ElementAt(i).Value;

                DataRow dr = rep.NewRow();
                dr["name"] = sr.Name;
                dr["cash_amount"] = sr.cash_amount;
                dr["card_amount"] = sr.card_amount;
                dr["total_amount"] = sr.cash_amount + sr.card_amount;
                dr["cash_price"] = sr.cash_total_price;
                dr["card_price"] = sr.card_total_price;
                dr["total_price"] = sr.cash_total_price + sr.card_total_price;
                rep.Rows.Add(dr);
            }

            string json = JsonConvert.SerializeObject(rep);            

            UriReportSource uriReportSource = new UriReportSource();
            uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//SellSouvenir.trdp";
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("header", header));
            report.ReportSource = uriReportSource;            
        }
        #endregion
    }

    public class SellRecord
    {
        public string Name { get; set; } = "";
        public int cash_amount { get; set; } = 0;
        public int card_amount { get; set; } = 0;
        public float cash_total_price { get; set; } = 0;
        public float card_total_price { get; set; } = 0;
        public SellRecord(string name)
        {
            Name = name;
        }
    }
}

/*



*/