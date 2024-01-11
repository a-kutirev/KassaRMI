using KassaLib;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Diagrams.Core;
using TextBox = System.Windows.Controls.TextBox;
using MessageBox = System.Windows.MessageBox;


namespace KassaRMI.Certrs
{
    /// <summary>
    /// Interaction logic for AddNewCert.xaml
    /// </summary>
    public partial class AddNewCert : INotifyPropertyChanged
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

        public AddNewCert()
        {
            InitializeComponent();
            this.DataContext = this;
            Model = new CertModel();
        }

        private void AddNewCertButton_Click(object sender, RoutedEventArgs e)
        {
            Model.Insert();

            MessageBox.Show($"Сертификат с номером {NomerCert.Text} зарегистрирован в базе");
        }

        public string NomerCertText;


        private void NomerCert_TextChanged(object sender, TextChangedEventArgs e)
        {
         
            TextBox t = sender as TextBox;
            if (t.Text.Length == 4)
            {
               NomerCertText = symbolEng(t.Text);
               t.Text = NomerCertText;
            }
        }

        private string symbolEng(string s)
        {
            string result = string.Empty;

            for (int i = 0; i < s.Length; i++)
            {
                char c = Convert.ToChar(s[i]);
                switch (c)
                {
                    case 'Ф': c = 'A'; break;
                    case 'И': c = 'B'; break;
                    case 'С': c = 'C'; break;
                    case 'В': c = 'D'; break;
                    case 'У': c = 'E'; break;
                    case 'А': c = 'F'; break;
                }
                result += c;
            }

            return result;
        }

        private void SelectExpoBt_Click(object sender, RoutedEventArgs e)
        {
            SelectExpoWindow win = new SelectExpoWindow();
            if((bool)win.ShowDialog())
            {
                ExpositionModel em = new ExpositionModel(win.IdExpo);
                Exponame = em.Expositionname;
                Model.Idexposition = em.Idexposition;
            }
        }
    }
}
