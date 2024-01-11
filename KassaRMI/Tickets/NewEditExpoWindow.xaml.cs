using KassaLib;
using KassaLib.Models;
using System.ComponentModel;
using System.Data;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for NewEditExpoWindow.xaml
    /// </summary>
    public partial class NewEditExpoWindow : INotifyPropertyChanged
    {
        #region Members
        public event PropertyChangedEventHandler PropertyChanged;
        private string par;
        private int id_e;

        private ExpositionModel m_expositionModel;

        public ExpositionModel ExpositionModel
        {
            get => m_expositionModel;
            set
            {
                m_expositionModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ExpostionModel"));
            }
        }

        #endregion

        #region Ctor
        public NewEditExpoWindow(string v, int v1)
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            par = v;
            id_e = v1;
            InitializeComponent();
            DialogResult = false;
            this.DataContext = this;

            string sql = "select * from exposition";
            DataTable dt = DBWrapper.Select(sql);

            ExpoCombo.ItemsSource = dt.DefaultView;
            ExpoCombo.SelectedValuePath = "idexposition";
            ExpoCombo.DisplayMemberPath = "expositionname";

            if(par == "new")
            {
                Header = "Новая экспозиция";
                m_expositionModel = new ExpositionModel();
                ExpoCombo.SelectedIndex = 0;
            }
            else
            {
                Header = "Редактирование экспозиции";
                m_expositionModel = new ExpositionModel(id_e);                      
            }            

            NonPersCB.IsChecked = !ExpositionModel.Expositionpersistent;
        }        
        #endregion

        #region Events
        private void SaveAndCloseBt_Click(object sender, RoutedEventArgs e)
        {
            if(par == "new")
            {
                m_expositionModel.Insert();
            }
            else
            {
                m_expositionModel.Update();
            }

            DialogResult = true;
            Close();
        }
        #endregion
    }
}
