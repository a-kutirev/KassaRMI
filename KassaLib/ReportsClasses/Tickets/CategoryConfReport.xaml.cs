using KassaLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using Telerik.Reporting;
using CheckBox = System.Windows.Controls.CheckBox;

namespace KassaLib.ReportsClasses.Tickets
{
    /// <summary>
    /// Interaction logic for CategoryConfReport.xaml
    /// </summary>
    public partial class CategoryConfReport
    {
        #region Members
        List<CheckBox> checked_list = new List<CheckBox>();
        #endregion

        #region Ctor
        public CategoryConfReport()
        {
            InitializeComponent();

            ReportDateFrom.SelectedDate = Option.CurrentDate;
            ReportDateTo.SelectedDate = Option.CurrentDate;

            string sql = "select * from category";

            DataTable tmp = DBWrapper.Select(sql);

            for (int i = 0; i < tmp.Rows.Count; i++)
            {
                int id = int.Parse(tmp.Rows[i]["idcategory"].ToString());
                string n = tmp.Rows[i]["categoryname"].ToString();

                CheckBox chk = new CheckBox();
                chk.Content = n;
                chk.CommandParameter = id;

                checked_list.Add(chk);
                CatListBox.Items.Add(chk);
            }
        }
        #endregion

        #region Events
        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = ReportDateFrom.DisplayDate;
            DateTime end = ReportDateTo.DisplayDate;

            bool showFree = (bool)ShowFree.IsChecked;
            bool shownotFree = (bool)ShowNotFree.IsChecked;

            string list = "";

            for (int i = 0; i < checked_list.Count; i++)
            {
                int id = (int)checked_list[i].CommandParameter;
                if ((bool)checked_list[i].IsChecked)
                    list += list == "" ? $"{id}" : $", {id}";
            }

            string condition = "";
            string header = "";

            if (showFree && shownotFree)
            {
                header = $"Отчет о посещении экспозиций категориями граждан по билетам за ";
                condition = "";
            }
            else if (showFree)
            {
                header = $"Отчет о посещении экспозиций категориями граждан по бесплатным билетам за ";
                condition = " and isnotfree = 0";
            }
            else if (shownotFree)
            {
                header = $"Отчет о посещении экспозиций категориями граждан по платным билетам за ";
                condition = " and isnotfree = 1";
            }

            string sql =
                        $"SELECT* FROM kassa.FreeTicketStat " +
                        $"where date(FreeTicketStatDate) between '{start.ToString("yyyy-MM-dd")}' and '{end.ToString("yyyy-MM-dd")}' " +
                        $"and idcategory in ({list}) {condition};";

            DataTable tmp = DBWrapper.Select(sql);

            Dictionary<int, int> idCat_amount = new Dictionary<int, int>();

            for(int i = 0; i < tmp.Rows.Count; i++)
            {
                int idCat = int.Parse(tmp.Rows[i]["idcategory"].ToString());
                int am_cur = int.Parse(tmp.Rows[i]["amount"].ToString());
                int idexpo = int.Parse(tmp.Rows[i]["idexposition"].ToString());
                DateTime d = DateTime.Parse(tmp.Rows[i]["FreeTicketStatDate"].ToString());

                if(!idCat_amount.ContainsKey(idCat))
                    idCat_amount.Add(idCat, 0);

                idCat_amount[idCat] += (idexpo == 15) ? am_cur * Option.GetVremCount(d) : am_cur;
            }

            DataTable rep = new DataTable();
            rep.Columns.Add("catname", typeof(string));
            rep.Columns.Add("amount", typeof(int));

            for(int i = 0; i < idCat_amount.Count; i++)
            {
                int id = idCat_amount.ElementAt(i).Key;
                int amount = idCat_amount.ElementAt(i).Value;

                CategoryModel category = new CategoryModel(id);

                DataRow dr = rep.NewRow();
                dr["catname"] = category.Categoryname;
                dr["amount"] = amount;
                rep.Rows.Add(dr);
            }

            string json = JsonConvert.SerializeObject(rep);
            header += $"период с " +
                    $"{start.ToString("dd MMMM yyyy")} г. по " +
                    $"{end.ToString("dd MMMM yyyy")} г.";
            //File.WriteAllText($"{Option.ReportFolder}CatConf.json", json);

            UriReportSource uriReportSource = new UriReportSource();
            uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//CategoryConf.trdp";
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("header", header));
            report.ReportSource = uriReportSource;
        }
        #endregion
    }
}
