using KassaLib;
using KassaLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
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
    /// Interaction logic for CheckCert.xaml
    /// </summary>
    public partial class CheckCert : INotifyPropertyChanged
    {
        private CertModel m_model;

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

        public CheckCert()
        {
            InitializeComponent();
            this.DataContext = this;
            DisActiveCert.Visibility = Visibility.Hidden;

        }

        private void SearchQR_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            //Если раскладка русская, меняем расшированные символы  qr кода на латинские
            string NomerCert = symbolEng(t.Text);
            Model = new CertModel(symbolEng(NomerCert));

            if (t.Text.Length == 4)
            {
                t.Text = NomerCert;


                if (Model.Nomer_cert != null)
                {
            
                    string pogash = Model.Isactive.ToString().ToLower() == "true" ? "Активен" : "Погашен";
                    string date_pogash = Model.Datapogasheniya == null ? "" : ((DateTime)Model.Datapogasheniya).ToString("yyyy-MM-dd");

                    CertInfo.Text = $"Сертификат с номером {Model.Nomer_cert}" + '\n' +
                                    $"Экспозиция {(new ExpositionModel(Model.Idexposition)).Expositionname}" + '\n' +
                                    $"Номиналом {Model.Nominal}" + '\n' +
                                    $"Выданный {(Model.Datavydachi).ToString("dd MMMM yyyy")}" + '\n' +
                                    pogash;
                    if (Model.Isactive == true)
                    {
                        DisActiveCert.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        CertInfo.Text = $"Сертификат с номером {Model.Nomer_cert}" + '\n' +
                                   $"Номиналом {Model.Nominal}" + '\n' +
                                   $"Экспозиция {(new ExpositionModel(Model.Idexposition)).Expositionname}" + '\n' +
                                   $"Выданный {(Model.Datavydachi).ToString("dd MMMM yyyy")}" + '\n' +
                                   pogash + '\n' +
                                   $"Дата погашения: {date_pogash}";
                        MessageBox.Show("Сертификат уже погашен!");
                    }
                
     
                }

                else
                {
                    MessageBox.Show("Сертификат с таким номером отстуствует в базе!");
                }
            }
        }

        private string symbolEng(string s)
        {
            string result = string.Empty;

            for (int i = 0; i <= s.Length - 1; i++)
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

        private void DisActiveCert_Click(object sender, RoutedEventArgs e)
        {
            Model.Isactive = false;
            Model.Datapogasheniya = DateTime.Today;
            Model.Update();
            MessageBox.Show("Сертификат погашен!");
            DisActiveCert.Visibility = Visibility.Hidden;
            SearchQR.Text = "";
        }
    }
}


