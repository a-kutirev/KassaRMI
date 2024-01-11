using System;
using System.Windows;
using Telerik.Reporting;
using Telerik.Windows.Controls;

namespace KassaLib.ReportsClasses.Souvenir
{
    /// <summary>
    /// Interaction logic for BalanceReportWindow.xaml
    /// </summary>
    public partial class BalanceReportWindow
    {
        #region Members
        #endregion

        #region Ctor
        public BalanceReportWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            ReportDate.SelectedValue = Option.CurrentDate;
        }
        #endregion

        #region Events
        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = (DateTime)ReportDate.SelectedValue;
            bool ShowZero = !(bool)NonShowZero.IsChecked;

            string json = Option.CalculateBalanceOnDate(dt, ShowZero);
            string header = $"Остаток товара на {dt.ToString("dd MMMM yyyy")} г.";

            UriReportSource uriReportSource = new UriReportSource();
            uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//BalanceSouvenir.trdp";
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("header", header));
            report.ReportSource = uriReportSource;
        }
        #endregion 
    }
}
