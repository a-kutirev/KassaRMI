using KassaLib;
using System;
using System.Data;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for TableByCategoryWindow.xaml
    /// </summary>
    public partial class TableByCategoryWindow
    {
        #region  Members
        #endregion

        #region Ctor
        public TableByCategoryWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            string sql =
                    "SELECT FreeTicketStat.*, category.categoryname, exposition.expositionname FROM kassa.FreeTicketStat " +
                    "inner join category on category.idcategory = FreeTicketStat.idcategory " +
                    "inner join exposition on exposition.idexposition = FreeTicketStat.idexposition " +
                    "order by FreeTicketStatDate desc ";

            DataTable dt = DBWrapper.Select(sql);

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime dte = DateTime.Parse(dt.Rows[i]["FreeTicketStatDate"].ToString());
                dt.Rows[i]["FreeTicketStatDate"] = dte.Date;
            }

            ByGridView.ItemsSource = dt.DefaultView;
        }
        #endregion

        #region Events
        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as RadButton).CommandParameter;

            EditCategorySellWindow win = new EditCategorySellWindow(id);
            win.ShowDialog();

            string sql =
                        "SELECT FreeTicketStat.*, category.categoryname, exposition.expositionname FROM kassa.FreeTicketStat " +
                        "inner join category on category.idcategory = FreeTicketStat.idcategory " +
                        "inner join exposition on exposition.idexposition = FreeTicketStat.idexposition " +
                        "order by FreeTicketStatDate desc; ";

            DataTable dt = DBWrapper.Select(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime dte = DateTime.Parse(dt.Rows[i]["FreeTicketStatDate"].ToString());
                dt.Rows[i]["FreeTicketStatDate"] = dte.Date;
            }

            ByGridView.ItemsSource = null;
            ByGridView.ItemsSource = dt.DefaultView;

            Option.RecalcTotal(win.dte);
        }
        private void CloseBt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
