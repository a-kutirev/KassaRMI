using KassaLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using Telerik.Reporting;
using Telerik.Windows.Controls;

namespace KassaLib.ReportsClasses.Tickets
{
    /// <summary>
    /// Interaction logic for CategoryReport.xaml
    /// </summary>
    public partial class CategoryReport
    {
        #region Members
        #endregion

        #region Ctor
        public CategoryReport()
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
            string sql = "";
            string type = "";
            string header = "";

            bool showFree = (bool)ShowFree.IsChecked;
            bool shownotFree = (bool)ShowNotFree.IsChecked;


            //************************************************************************
            //select FreeTicketStat.*, category.categoryname, exposition.expositionname from FreeTicketStat
            //inner join category on FreeTicketStat.idcategory = category.idcategory
            //inner join exposition on FreeTicketStat.idexposition = exposition.idexposition
            //where date(FreeTicketStatDate) between '2022-03-01' and '2022-03-31'
            //************************************************************************

            string condition = "";
            header = "";

            if(showFree && shownotFree)
            {
                header = $"Отчет о посещении экспозиций категориями граждан по билетам за ";
                condition = "";
            }
            else if (showFree)
            {
                header = $"Отчет о посещении экспозиций категориями граждан по бесплатным билетам за ";
                condition = " and isnotfree = 0";
            }
            else if(shownotFree)
            {
                header = $"Отчет о посещении экспозиций категориями граждан по платным билетам за ";
                condition = " and isnotfree = 1";
            }

            if((bool)rb1.IsChecked)
            {
                string t1 = ((DateTime)ReportDate.SelectedValue).ToString("yyyy-MM-dd");

                sql =
                    $"select FreeTicketStat.*, category.categoryname, exposition.expositionname from FreeTicketStat " +
                    $"inner join category on FreeTicketStat.idcategory = category.idcategory " +
                    $"inner join exposition on FreeTicketStat.idexposition = exposition.idexposition " +
                    $"where (date(FreeTicketStatDate) = '{t1}'){condition}";

                type = "day";
                header += $"{((DateTime)ReportDate.SelectedValue).ToString("dd MMMM yyyy")} г.";
            }
            else
            {
                string t1 = ((DateTime)ReportDateFrom.SelectedValue).ToString("yyyy-MM-dd");
                string t2 = ((DateTime)ReportDateTo.SelectedValue).ToString("yyyy-MM-dd");

                sql =
                    $"select FreeTicketStat.*, category.categoryname, exposition.expositionname from FreeTicketStat " +
                    $"inner join category on FreeTicketStat.idcategory = category.idcategory " +
                    $"inner join exposition on FreeTicketStat.idexposition = exposition.idexposition " +
                    $"where (date(FreeTicketStatDate) between '{t1}' and '{t2}') {condition}";

                type = "period";
                header += $"период с " +
                    $"{((DateTime)ReportDateFrom.SelectedValue).ToString("dd MMMM yyyy")} г. по " +
                    $"{((DateTime)ReportDateTo.SelectedValue).ToString("dd MMMM yyyy")} г.";
            }        

            DataTable tmp = DBWrapper.Select(sql);            
            SortedDictionary<int, List<CategoryReportItem>> expo_catItem = new SortedDictionary<int, List<CategoryReportItem>>();

            for (int i = 0; i < tmp.Rows.Count; i++)
            {
                int expoid = int.Parse(tmp.Rows[i]["idexposition"].ToString());
                if (!expo_catItem.ContainsKey(expoid))
                    expo_catItem.Add(expoid, new List<CategoryReportItem>());
            }

            for (int i = 0; i < tmp.Rows.Count; i++)
            {
                int idCat = int.Parse(tmp.Rows[i]["idcategory"].ToString());
                int expoid = int.Parse(tmp.Rows[i]["idexposition"].ToString());                
                int amount_current = int.Parse(tmp.Rows[i]["amount"].ToString());
                DateTime dat = DateTime.Parse(tmp.Rows[i]["FreeTicketStatDate"].ToString());

                bool exist = false;
                for(int j = 0; j < expo_catItem[expoid].Count; j++)
                {
                    if (expo_catItem[expoid][j].IdCat == idCat)
                    {
                        exist = true;
                        int a = expo_catItem[expoid][j].Amount;
                        expo_catItem[expoid][j].Amount = (expoid == 15) ? 
                            a + amount_current * Option.GetVremCount(dat) : a + amount_current;
                        break;
                    }
                }

                if(!exist)
                {
                    int a = (expoid == 15) ? amount_current * Option.GetVremCount(dat) : amount_current;
                    CategoryReportItem cri = new CategoryReportItem()
                    {
                        IdCat = idCat,
                        Amount = a
                    };
                    expo_catItem[expoid].Add(cri);
                }
            }

            DataTable res = new DataTable();
            res.Columns.Add("idexposition", typeof(int));
            res.Columns.Add("expositionname", typeof(string));
            res.Columns.Add("categoryname", typeof(string));
            res.Columns.Add("amount", typeof(int));

            for(int i = 0; i < expo_catItem.Count; i++)
            {               
                int idExpo = expo_catItem.ElementAt(i).Key;
                ExpositionModel em = new ExpositionModel(idExpo);

                for (int j = 0; j < expo_catItem[idExpo].Count; j++)
                {
                    DataRow row = res.NewRow();
                    row["idexposition"] = idExpo;
                    row["expositionname"] = em.Expositionname;

                    int idCat = expo_catItem[idExpo][j].IdCat;
                    CategoryModel cm = new CategoryModel(idCat);
                    row["categoryname"] = cm.Categoryname;
                    row["amount"] = expo_catItem[idExpo][j].Amount;

                    res.Rows.Add(row);
                }
            }

            string json = JsonConvert.SerializeObject(res);

            UriReportSource uriReportSource = new UriReportSource();
            uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//Category.trdp";
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("header", header));
            report.ReportSource = uriReportSource;
        }
        #endregion
    }

    public class CategoryReportItem
    {
        public int IdCat { get; set; } = 0;
        public int Amount { get; set; } = 0;
        public CategoryReportItem() {}
    }
}
