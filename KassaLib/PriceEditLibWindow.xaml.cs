using KassaLib.Models;
using System.ComponentModel;
using System.Windows;

namespace KassaLib
{
    /// <summary>
    /// Interaction logic for PriceEditLibWindow.xaml
    /// </summary>
    public partial class PriceEditLibWindow : INotifyPropertyChanged
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
        public PriceEditLibWindow(NomenklaturaModel m)
        {
            InitializeComponent();
            this.DataContext = this;

            NomenklaturaModel = m;
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
