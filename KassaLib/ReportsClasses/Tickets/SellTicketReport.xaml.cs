using Newtonsoft.Json;
using System;
using System.Data;
using System.Windows;
using Telerik.Reporting;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Flow.Model.Fields;

namespace KassaLib.ReportsClasses.Tickets
{
    /// <summary>
    /// Interaction logic for SellTicketReport.xaml
    /// </summary>
    public partial class SellTicketReport
    {
        #region Members
        #endregion

        #region Ctor
        public SellTicketReport()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            ReportDateFrom.SelectedValue = Option.CurrentDate;
            ReportDateTo.SelectedValue = Option.CurrentDate;
        }
        #endregion

        #region Events
        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime t1 = (DateTime)ReportDateFrom.SelectedValue;
            DateTime t2 = (DateTime)ReportDateTo.SelectedValue;

            string sql = "";
            string header = "";

            sql = $"select * from free_ticket_sell where date_ between '{t1.ToString("yyyy-MM-dd")}' and '{t2.ToString("yyyy-MM-dd")}'";


            DataTable dt = DBWrapper.Select(sql);

            DataTable tmp = new DataTable();
            tmp.Columns.Add("date_", typeof(string));
            tmp.Columns.Add("amount", typeof(string));

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = tmp.NewRow();
                row["date_"] = (DateTime.Parse(dt.Rows[i]["date_"].ToString())).ToString("dd MMMM yyyy") + " г.";

                if((bool)ShowFree.IsChecked && (bool)ShowNotFree.IsChecked)
                {
                    header = $"Отчет о выданных билетах за период с " +
                        $"{t1.ToString("dd.MM.yyyy")} по " +
                        $"{t2.ToString("dd.MM.yyyy")}";
                    row["amount"] = dt.Rows[i]["amount"].ToString();
                }
                else if ((bool)ShowFree.IsChecked)
                {
                    header = $"Отчет о выданных бесплатных билетах за период с " +
                        $"{t1.ToString("dd.MM.yyyy")} по " +
                        $"{t2.ToString("dd.MM.yyyy")}";
                    row["amount"] = dt.Rows[i]["amount_free"].ToString();
                }
                else if ((bool)ShowNotFree.IsChecked)
                {
                    header = $"Отчет о выданных платных билетах за период с " +
                        $"{t1.ToString("dd.MM.yyyy")} по " +
                        $"{t2.ToString("dd.MM.yyyy")}";
                    row["amount"] = dt.Rows[i]["amount_notfree"].ToString();
                }                

                tmp.Rows.Add(row);
            }

            string json = JsonConvert.SerializeObject(tmp);

            UriReportSource uriReportSource = new UriReportSource();
            uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//SellTicket.trdp";
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("header", header));
            report.ReportSource = uriReportSource;
        }
        #endregion
    }
}
