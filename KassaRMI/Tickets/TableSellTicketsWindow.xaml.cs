using KassaLib;
using System.Data;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for TableSellTicketsWindow.xaml
    /// </summary>
    public partial class TableSellTicketsWindow
    {
        #region Members
        #endregion

        #region Ctor
        public TableSellTicketsWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            string sql = "select * from free_ticket_sell order by date_ desc";
            DataTable dt = DBWrapper.Select(sql);

            SellGridView.ItemsSource = dt.DefaultView;
        }
        #endregion


        #region Events
        private void CloseBt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            int id_s = (int)(sender as RadButton).CommandParameter;
            SellTicketWindow win = new SellTicketWindow(Option.CurrentDate, "edit", id_s);
            win.ShowDialog();



            string sql = "select * from free_ticket_sell order by date_ desc";
            DataTable dt = DBWrapper.Select(sql);
            SellGridView.ItemsSource = null;
            SellGridView.ItemsSource = dt.DefaultView;
        }
        #endregion 
    }
}
