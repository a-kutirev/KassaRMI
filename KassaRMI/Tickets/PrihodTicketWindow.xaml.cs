using KassaLib;
using KassaLib.Models;
using System.ComponentModel;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for PrihodTicketWindow.xaml
    /// </summary>
    public partial class PrihodTicketWindow : INotifyPropertyChanged
    {
        #region Members
        public event PropertyChangedEventHandler PropertyChanged;
        private string par;
        private int id_pr;

        private NewTicketsModel m_newTicketsModel;

        public NewTicketsModel NewTicketsModel
        {
            get => m_newTicketsModel;
            set
            {
                m_newTicketsModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NewTicketsModel"));
            }
        }
        #endregion

        #region Ctor
        public PrihodTicketWindow(string v, int v1)
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            this.par = v;
            id_pr = v1;
            InitializeComponent();
            this.DataContext = this;

            if(par == "new")
            {
                NewTicketsModel = new NewTicketsModel();
            }
            else
            {
                NewTicketsModel = new NewTicketsModel(id_pr);
            }

            NewNumCB.IsChecked = NewTicketsModel.Usenewnumeration == 1;
        }
        #endregion

        #region Events
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            m_newTicketsModel.Usenewnumeration = (bool)NewNumCB.IsChecked ? 1 : 0;

            if (par == "new")
            {
                m_newTicketsModel.Insert();
            }
            else
            {
                m_newTicketsModel.Update();
            }

            MessageBox.Show("Данные сохранены");
            Close();
        }
        #endregion
    }
}
