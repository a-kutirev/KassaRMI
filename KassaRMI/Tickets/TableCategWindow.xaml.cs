using KassaLib;
using System.Data;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for TableCategWindow.xaml
    /// </summary>
    public partial class TableCategWindow
    {
        #region Members
        #endregion

        #region Ctor
        public TableCategWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            string sql = "select * from category";
            DataTable tmp = DBWrapper.Select(sql);

            CategGridView.ItemsSource = tmp.DefaultView;
        }
        #endregion

        #region Events
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            string nameBt = (sender as RadButton).Name;
            NewEditCategoryWindow win;

            if (nameBt == "NewCategBt")
            {
                win = new NewEditCategoryWindow("new", -1);
            }
            else
            {
                int id = (int)(sender as RadButton).CommandParameter;
                win = new NewEditCategoryWindow("edit", id);
            }

            if((bool)win.ShowDialog())
            {

            }            
        }
        #endregion
    }
}
