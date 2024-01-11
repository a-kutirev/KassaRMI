using KassaLib;
using KassaLib.Models;
using KassaLib.ReportsClasses.Souvenir;
using KassaLib.ReportsClasses.Tickets;
using KassaRMI.Certrs;
using KassaRMI.Souvenirs;
using KassaRMI.Tickets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace KassaRMI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members
        #endregion

        #region Ctor
        public MainWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();

            InitializeComponent();

            string sql = "select * from options";
            Option.optionsAll = (List<OptionsModel>)DBWrapper.Select(sql).ToList<OptionsModel>();

            if (!RegistryOptions.Check())
                RegistryOptions.CreateKey();

            if(!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Reports\\"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Reports\\");

            Option.ReportFolder = AppDomain.CurrentDomain.BaseDirectory + "Reports\\";

            CurDatePicker.SelectedDate = DateTime.Now;
            Option.CurrentDate = (DateTime)CurDatePicker.SelectedDate;

            Option.CalculateBalance();

            int c = Option.GetVremCount(new DateTime(2022, 03, 30));
        }
        #endregion

        #region Events
        private void SellSouvenirBt_Click(object sender, RoutedEventArgs e)
            => (new SellSouvenirWindow("new", -1)).ShowDialog();
        private void PrihodSouvenirBt_Click(object sender, RoutedEventArgs e)
            => (new PrihodSouvenirWindow("new", -1)).ShowDialog();

        private void CurDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Option.CurrentDate = (DateTime)(sender as RadDatePicker).SelectedDate;
        }

        private void ShowPicker_Checked(object sender, RoutedEventArgs e)
        {
            if(!(bool)(sender as CheckBox).IsChecked)
            {
                CurDatePicker.SelectedDate = DateTime.Now;
                Option.CurrentDate = (DateTime)CurDatePicker.SelectedDate;
            }
        }

        private void SellTicketBt_Click(object sender, RoutedEventArgs e)
        {
            SellTicketWindow ftw = new SellTicketWindow(Option.CurrentDate, "new", -1);
            ftw.ShowDialog();
        }

        private void SouvNomBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            SouvTableContentMenu.IsOpen = false;
            (new TableNomenklaturaWindow()).ShowDialog();
        }
        private void SouvPrihBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            SouvTableContentMenu.IsOpen = false;
            (new Souvenirs.TablePrihodWindow()).ShowDialog();
        }
        private void SouvSellBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            SouvTableContentMenu.IsOpen = false;
            (new TableSellWindow()).ShowDialog();
        }

        private void RadRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            WindowCollection windows = Application.Current.Windows;
            foreach (Window window in windows)
            {
                if (window.Content is CategTicketWindow)
                {
                    window.Close();
                }
            }

            (new CategTicketWindow()).Show();
        }

        private void PrihodTicketBt_Click(object sender, RoutedEventArgs e)
        {
            (new PrihodTicketWindow("new", -1)).ShowDialog();
        }

        private void ExpoBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TicketContextMenu.IsOpen = false;
            TableExpoWindow win = new TableExpoWindow();
            win.ShowDialog();
        }

        private void CategBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TicketContextMenu.IsOpen = false; 
            TableCategWindow win = new TableCategWindow();
            win.ShowDialog();
        }

        private void TicketPrihodBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TicketContextMenu.IsOpen = false;
            Tickets.TablePrihodWindow win = new Tickets.TablePrihodWindow();
            win.ShowDialog();
        }

        private void SellTicketsBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TicketContextMenu.IsOpen = false;
            TableSellTicketsWindow win = new TableSellTicketsWindow();
            win.ShowDialog();
        }

        private void ByCategBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TicketContextMenu.IsOpen = false;
            TableByCategoryWindow win = new TableByCategoryWindow();
            win.ShowDialog();
        }

        private void SellSouvBt_Click(object sender, RoutedEventArgs e)
        {
            SouvReportContextMenu.IsOpen = false;
            SellSouvReportWindow win = new SellSouvReportWindow();
            win.ShowDialog();
        }

        private void BalanceSouvBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            SouvReportContextMenu.IsOpen = false;
            BalanceReportWindow win = new BalanceReportWindow();
            win.ShowDialog();
        }

        private void MoveBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            SouvReportContextMenu.IsOpen = false;
            MoveReportWindow win = new MoveReportWindow();
            win.ShowDialog();
        }

        private void StatBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TickReportContextMenu.IsOpen = false;
            CategoryReport win = new CategoryReport();
            win.ShowDialog();
        }

        private void SellTicketRepBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TickReportContextMenu.IsOpen = false;
            SellTicketReport win = new SellTicketReport();
            win.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            WindowCollection windows = Application.Current.Windows;
            foreach (Window window in windows)
            {
                if (window.Content is CategTicketWindow)
                    window.Close();
            }
        }

        private void SellTicketRepBtExt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TickReportContextMenu.IsOpen = false;
            SellReportExt win = new SellReportExt();
            win.ShowDialog();
        }

        private void PriceLabelBtBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            SouvReportContextMenu.IsOpen = false;
            SelectToPrintLabelWindow win = new SelectToPrintLabelWindow();
            win.ShowDialog();
        }

        private void MoveConfigBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            SouvReportContextMenu.IsOpen = false;
            MoveConfigReportWindow win = new MoveConfigReportWindow();
            win.ShowDialog();
        }

        private void StatConfBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TickReportContextMenu.IsOpen = false;
            CategoryConfReport win = new CategoryConfReport();
            win.ShowDialog();
        }

        private void OptionBt_Click(object sender, RoutedEventArgs e)
        {
            OptionWindow win = new OptionWindow();
            win.ShowDialog();
        }

        private void TicketBalanceBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TickReportContextMenu.IsOpen = false;
            TicketBalanceReportWindow win = new TicketBalanceReportWindow();
            win.ShowDialog();
        }

        private void StatExpoBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TickReportContextMenu.IsOpen = false;
            ExpoStatReport win = new ExpoStatReport();
            win.ShowDialog();
        }

        private void StatCatExpoBt_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TickReportContextMenu.IsOpen = false;
            ExpoCatStatReport win = new ExpoCatStatReport();
            win.ShowDialog();
        }

        private void AddCert_Btn_Click(object sender, RoutedEventArgs e)
        {
            AddNewCert win = new AddNewCert();
            win.ShowDialog();
        }

        private void ScanCert_Btn_Click(object sender, RoutedEventArgs e)
        {
            CheckCert win = new CheckCert();
            win.ShowDialog();
        }

        private void TableCert_Btn_Click(object sender, RoutedEventArgs e)
        {
            TableCert win = new TableCert();
            win.ShowDialog();
        }
    }
    #endregion
}
