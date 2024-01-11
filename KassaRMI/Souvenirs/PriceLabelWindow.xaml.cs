using KassaLib.Models;
using System.ComponentModel;
using System.Windows;

namespace KassaRMI.Souvenirs
{
    /// <summary>
    /// Interaction logic for PriceLabelWindow.xaml
    /// </summary>
    public partial class PriceLabelWindow : INotifyPropertyChanged
    {
        #region Members

        NomenklaturaModel m_nomenklaturaModel;

        public NomenklaturaModel NomenklaturaModel
        {
            get => m_nomenklaturaModel;
            set
            {
                m_nomenklaturaModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NomenklaturaModel"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Ctor
        public PriceLabelWindow(NomenklaturaModel m)
        {
            InitializeComponent();
            this.DataContext = this;

            NomenklaturaModel = m;
        }        
        #endregion

        #region Events
        private void SaveAddBt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
