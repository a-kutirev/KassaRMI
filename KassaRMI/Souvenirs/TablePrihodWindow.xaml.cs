using KassaLib;
using System.Data;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Souvenirs
{
    /// <summary>
    /// Interaction logic for TablePrihodWindow.xaml
    /// </summary>
    public partial class TablePrihodWindow
    {
        #region Members
        #endregion

        #region Ctor
        public TablePrihodWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            string sql =
                "SELECT prihod.*, nomenklatura.nomenklaturaname FROM prihod " +
                "inner join nomenklatura on nomenklatura.idnomenklatura = prihod.idnomenclatura " +
                "order by date_ desc";

            DataTable tmp = DBWrapper.Select(sql);
            PrihodGridView.ItemsSource = tmp.DefaultView;
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

            PrihodSouvenirWindow win = new PrihodSouvenirWindow("edit", id);
            win.ShowDialog();

            string sql =
                "SELECT prihod.*, nomenklatura.nomenklaturaname FROM prihod " +
                "inner join nomenklatura on nomenklatura.idnomenklatura = prihod.idnomenclatura " +
                "order by date_ desc";

            DataTable tmp = DBWrapper.Select(sql);
            PrihodGridView.ItemsSource = tmp.DefaultView;

        }
        #endregion
    }
}
