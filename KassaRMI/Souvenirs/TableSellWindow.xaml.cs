using KassaLib;
using System.Data;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Souvenirs
{
    /// <summary>
    /// Interaction logic for TableSellWindow.xaml
    /// </summary>
    public partial class TableSellWindow
    {
        #region Members
        #endregion

        #region Ctor
        public TableSellWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            string sql = "SELECT sell.*,  nomenklatura.nomenklaturaname FROM kassa.sell " +
                "inner join kassa.nomenklatura on nomenklatura.idnomenklatura = sell.idnomenclatura " +
                "order by date_ desc";

            DataTable dt = DBWrapper.Select(sql);
            SellGridView.ItemsSource = dt.DefaultView;
        }
        #endregion

        #region Events
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as RadButton).CommandParameter;

            SellSouvenirWindow win = new SellSouvenirWindow("edit", id);
            win.ShowDialog();

            string sql = "SELECT sell.*,  nomenklatura.nomenklaturaname FROM kassa.sell " +
                         "inner join kassa.nomenklatura on nomenklatura.idnomenklatura = sell.idnomenclatura " +
                         "order by date_ desc";

            DataTable dt = DBWrapper.Select(sql);
            SellGridView.ItemsSource = dt.DefaultView;
        }

        #endregion
    }
}
