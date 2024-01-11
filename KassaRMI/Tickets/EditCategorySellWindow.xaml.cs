using KassaLib;
using KassaLib.Models;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for EditCategorySellWindow.xaml
    /// </summary>
    public partial class EditCategorySellWindow: INotifyPropertyChanged
    {
        #region Members
        public event PropertyChangedEventHandler PropertyChanged;

        private FreeTicketStatModel m_freeTicketStatModel;


        public DateTime dte { get; set; }
        public FreeTicketStatModel FreeTicketStatModel
        {
            get => m_freeTicketStatModel;
            set
            {
                m_freeTicketStatModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FreeTicketStatModel"));
            }
        }
        #endregion

        #region Ctor
        public EditCategorySellWindow(int id)
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();
            this.DataContext = this;
            DialogResult = false;

            string sql = "select idexposition, expositionname from exposition";
            DataTable tmp = DBWrapper.Select(sql);
            ExpoCombo.ItemsSource = tmp.DefaultView;
            ExpoCombo.DisplayMemberPath = "expositionname";
            ExpoCombo.SelectedValuePath = "idexposition";

            sql = "select idcategory, categoryname from category";
            tmp = DBWrapper.Select(sql);
            CategCombo.ItemsSource = tmp.DefaultView;
            CategCombo.DisplayMemberPath = "categoryname";
            CategCombo.SelectedValuePath = "idcategory";

            FreeTicketStatModel = new FreeTicketStatModel(id);

        }        
        #endregion

        #region Events
        private void SaveAndCloseBt_Click(object sender, RoutedEventArgs e)
        {
            m_freeTicketStatModel.Update();
            dte = m_freeTicketStatModel.FreeTicketStatDate;

            MessageBox.Show("Данные сохранены");

            Close();
        }
        #endregion
    }
}
