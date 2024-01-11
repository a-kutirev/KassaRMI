using KassaLib;
using KassaLib.Models;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Telerik.Windows.Controls;

namespace KassaRMI.Souvenirs
{
    /// <summary>
    /// Interaction logic for SelectTovarWindow.xaml
    /// </summary>
    public partial class SelectTovarWindow
    {
        #region Members

        public NomenklaturaModel Model { get; set; }
        private bool ShowZeroAmount = false;
        private bool loaded = false;

        #endregion

        #region Ctor
        public SelectTovarWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();
            DialogResult = false;
            loaded = true;
            FillData();
        }
        #endregion

        #region Events
        private void RadGridView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRow row = ((sender as RadGridView).SelectedItems[0] as DataRowView).Row;
            int id = (int)row["idnomenklatura"];
            Model = new NomenklaturaModel(id);
            DialogResult = true;
            Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ShowZeroAmount = (bool)(sender as CheckBox).IsChecked;
            FillData();
        }

        private void FillData()
        {
            if (!loaded) return;

            string sql;

            if (ShowZeroAmount)
                sql = "select nomenklatura.idnomenklatura, nomenklatura.nomenklaturaname, price.price, nomenklatura.balance" +
                             " from nomenklatura inner join price on nomenklatura.idnomenklatura = price.idnomenclatura " +
                             "where nomenklatura.balance > 0; ";
            else
                sql = "select nomenklatura.idnomenklatura, nomenklatura.nomenklaturaname, price.price, nomenklatura.balance" +
                             " from nomenklatura inner join price on nomenklatura.idnomenklatura = price.idnomenclatura ";

            DataTable tmp = DBWrapper.Select(sql);
            SelectTovarGridView.ItemsSource = tmp.DefaultView;
        }
        #endregion
    }
}
