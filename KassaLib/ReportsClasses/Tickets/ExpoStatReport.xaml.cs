using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Reporting;
using Telerik.Windows.Controls;
using CheckBox = System.Windows.Controls.CheckBox;
using TextBox = System.Windows.Controls.TextBox;

namespace KassaLib.ReportsClasses.Tickets
{
    /// <summary>
    /// Interaction logic for ExpoStatReport.xaml
    /// </summary>
    public partial class ExpoStatReport
    {
        #region Members
        List<CheckBox> checked_list = new List<CheckBox>();
        #endregion

        #region Ctor
        public ExpoStatReport()
        {
            InitializeComponent();

            ReportDateFrom.SelectedDate = Option.CurrentDate;
            ReportDateTo.SelectedDate = Option.CurrentDate;

            string sql = "SELECT * FROM exposition";
            DataTable tmp = DBWrapper.Select(sql);

            for (int i = 0; i < tmp.Rows.Count; i++)
            {
                int id = int.Parse(tmp.Rows[i]["idexposition"].ToString());
                string n = tmp.Rows[i]["expositionname"].ToString();

                CheckBox chk = new CheckBox();
                chk.Content = n;
                chk.CommandParameter = id;

                checked_list.Add(chk);
                ExpoListBox.Items.Add(chk);
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

            bool showFree = (bool)ShowFree.IsChecked;
            bool shownotFree = (bool)ShowNotFree.IsChecked;

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

            string condition = "";
            string header = "";

            if (showFree && shownotFree)
            {
                header = $"Статистика посещений экспозиций ";
                condition = "";
            }
            else if (showFree)
            {
                header = $"Статистика посещений по бесплатным билетам экспозиций ";
                condition = " and isnotfree = 0";
            }
            else if (shownotFree)
            {
                header = $"Статистика посещений по платным билетам экспозиций ";
                condition = " and isnotfree = 1";
            }

            string sql =
                $"select exposition.expositionname, FreeTicketStatDate, sum(amount) as amount from FreeTicketStat " +
                $"inner join exposition on exposition.idexposition = FreeTicketStat.idexposition " +
                $"where (FreeTicketStatDate between '{datefrom}' and '{dateto}') " +
                $"and FreeTicketStat.idexposition in ({list}) {condition} " +
                $"group by FreeTicketStat.idexposition, FreeTicketStatDate";

            DataTable tmp = DBWrapper.Select(sql);

            DataTable outTable = new DataTable();
            outTable.Columns.Add("expositionname", typeof(string));
            outTable.Columns.Add("FreeTicketStatDate", typeof(string));
            outTable.Columns.Add("amount", typeof(int));

            for(int i = 0; i < tmp.Rows.Count; i++)
            {
                DataRow dr = outTable.NewRow();

                dr["expositionname"] = tmp.Rows[i]["expositionname"].ToString();
                DateTime tmpDate = DateTime.Parse(tmp.Rows[i]["FreeTicketStatDate"].ToString());
                dr["FreeTicketStatDate"] = tmpDate.ToString("dd MMMM");
                dr["amount"] = int.Parse(tmp.Rows[i]["amount"].ToString());

                outTable.Rows.Add(dr);
            }

            string path = $"{AppDomain.CurrentDomain.BaseDirectory}/Reports/ExpoStat.json";
            string json = JsonConvert.SerializeObject(outTable);

            header += $"за период с {datefrom} по {dateto}";

            UriReportSource uriReportSource = new UriReportSource();
            uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//ExpoStatReport.trdp";
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("header", header));
            report.ReportSource = uriReportSource;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string t = (sender as TextBox).Text;
            ExpoListBox.Items.Clear();
            for (int i = 0; i < checked_list.Count; i++)
            {
                string cb = checked_list[i].Content.ToString().ToUpper();

                if (cb.Contains(t.ToUpper()))
                    ExpoListBox.Items.Add(checked_list[i]);
            }
        }
        #endregion
    }
}
