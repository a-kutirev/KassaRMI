using KassaLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Telerik.Windows.Controls;

namespace KassaRMI.Certrs
{
    /// <summary>
    /// Interaction logic for EditTableCert.xaml
    /// </summary>
    public partial class EditTableCert : INotifyPropertyChanged
    {
        private CertModel m_model;
        private string m_exponame;

        public event PropertyChangedEventHandler PropertyChanged;

        public CertModel Model
        {
            get => m_model;
            set
            {
                m_model = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Model"));
            }

        }

        public string Exponame
        {
            get => m_exponame;
            set
            {
                m_exponame = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Exponame"));
            }
        }

        public EditTableCert(CertModel m)
        {
            InitializeComponent();
            this.DataContext = this;
            Model = m;

            DialogResult = false;
            Exponame = (new ExpositionModel(m.Idexposition)).Expositionname;
        }

        public EditTableCert()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Model.Update();
            DialogResult = true;
            Close();
        }

        private void SelectExpoBt_Click(object sender, RoutedEventArgs e)
        {
            SelectExpoWindow win = new SelectExpoWindow();
            if ((bool)win.ShowDialog())
            {
                ExpositionModel em = new ExpositionModel(win.IdExpo);
                Exponame = em.Expositionname;
                Model.Idexposition = em.Idexposition;
            }
        }
    }
}
