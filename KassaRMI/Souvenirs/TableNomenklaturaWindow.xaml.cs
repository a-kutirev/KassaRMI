using KassaLib;
using System.Data;
using System.Windows;
using System.Windows.Controls;

using Telerik.Windows.Controls;

namespace KassaRMI.Souvenirs
{
    /// <summary>
    /// Interaction logic for TableNomenklaturaWindow.xaml
    /// </summary>
    public partial class TableNomenklaturaWindow
    {
        #region Members

        private bool loaded = false;
        private bool ShowZeroNom = false;

        #endregion

        #region Ctor
        public TableNomenklaturaWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            loaded = true;
            SetDataGrid();
        }
        #endregion

        #region Events
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!loaded) return;
            ShowZeroNom = (bool)(sender as CheckBox).IsChecked;
            SetDataGrid();
        }
        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            string btName = (sender as RadButton).Name;
            string param = btName == "NewNomenklaturaBt" ? "new" : "edit";

            int id = btName == "NewNomenklaturaBt" ? -1 : (int)(sender as RadButton).CommandParameter;


            NewEditNomenklaturaWin win = new NewEditNomenklaturaWin(param, id);
            win.ShowDialog();

            SetDataGrid();
        }
        private void SetDataGrid()
        {
            if (!loaded) return;

            string sql = "";
            if(!ShowZeroNom)
            {
                sql =
                "SELECT nomenklatura.*, price.price FROM nomenklatura " +
                "inner join price on price.idnomenclatura = nomenklatura.idnomenklatura";
            }
            else
            {
                sql = 
                "SELECT nomenklatura.*, price.price FROM nomenklatura " +
                "inner join price on price.idnomenclatura = nomenklatura.idnomenklatura " +
                "where balance > 0";
            }

            DataTable tmp = DBWrapper.Select(sql);
            NomenklaturaGridView.ItemsSource = null;
            NomenklaturaGridView.ItemsSource = tmp.DefaultView;
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
