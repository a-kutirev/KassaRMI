using KassaLib;
using KassaLib.Models;
using System.ComponentModel;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for NewEditCategoryWindow.xaml
    /// </summary>
    public partial class NewEditCategoryWindow : INotifyPropertyChanged
    {
        #region Members
        public event PropertyChangedEventHandler PropertyChanged;
        private string par;
        private int id_cat;

        private CategoryModel m_categoryModel;

        public CategoryModel CategoryModel
        {
            get => m_categoryModel;
            set
            {
                m_categoryModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CategoryModel"));
            }
        }
        #endregion

        #region Ctor
        public NewEditCategoryWindow(string v, int v1)
        {

            LocalizationManager.Manager = new CustomLocalizationManager();
            par = v;
            id_cat = v1;

            InitializeComponent();
            this.DataContext = this;
            DialogResult = false;

            if(par == "new")
            {
                Header = "Новая категория";
                m_categoryModel = new CategoryModel();
            }
            else
            {
                Header = "Редактирование категории";
                m_categoryModel = new CategoryModel(id_cat);
            }

            
        }        
        #endregion

        #region Events
        private void SaveAndCloseBt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
