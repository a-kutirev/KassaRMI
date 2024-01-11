using KassaLib;
using KassaLib.Models;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Telerik.Windows.Controls;

namespace KassaRMI.Souvenirs
{
    /// <summary>
    /// Interaction logic for PrihodSouvenirWindow.xaml
    /// </summary>
    public partial class PrihodSouvenirWindow : INotifyPropertyChanged
    {
        #region Members
        public event PropertyChangedEventHandler PropertyChanged;

        private PrihodModel m_prihodModel;
        private NomenklaturaModel m_nomenklaturaModel;
        private PriceModel m_priceModel;

        private string par;
        private int id_s;

        private DateTime m_dateCurrent = Option.CurrentDate;

        public PrihodModel PrihodModel
        {
            get => m_prihodModel;
            set
            {
                m_prihodModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PrihodModel"));
            }
        }
        public NomenklaturaModel NomenklaturaModel
        {
            get => m_nomenklaturaModel;
            set
            {
                m_nomenklaturaModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NomenklaturaModel"));
            }
        }
        public PriceModel PriceModel
        {
            get => m_priceModel;
            set
            {
                m_priceModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PriceModel"));
            }
        }
        #endregion

        #region Ctor
        public PrihodSouvenirWindow(string param, int id)
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            this.par = param;
            this.id_s = id;

            InitializeComponent();
            this.DataContext = this;
            DialogResult = false;

            if (par == "new")
            {
                PrihodModel = new PrihodModel();
            }
            else
            {
                PrihodModel = new PrihodModel(id_s);
                NomenklaturaModel = new NomenklaturaModel(PrihodModel.Idnomenclatura);
                PriceModel = new PriceModel(DateTime.Now, PrihodModel.Idnomenclatura);
                PrihodTextBox.Text = PrihodModel.Prihod_amount.ToString();
                m_dateCurrent = PrihodModel.Date_;
                SelectTovarBt.IsEnabled = false;
                SaveBt.Visibility = Visibility.Collapsed;
            }

            DateLabel.Content = m_dateCurrent.ToString("dd MMMM yyyy") + " г.";
        }        
        #endregion

        #region Events
        private void SelectTovarBt_Click(object sender, RoutedEventArgs e)
        {
            SelectTovarWindow win = new SelectTovarWindow();
            if((bool)win.ShowDialog())
            {
                NomenklaturaModel = win.Model;
                PriceModel = new PriceModel(m_dateCurrent, NomenklaturaModel.Idnomenklatura);

                m_prihodModel.Idnomenclatura = NomenklaturaModel.Idnomenklatura;
                m_prihodModel.Date_ = m_dateCurrent;

                PrihodTextBox.Focus();
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text != "0" || (sender as TextBox).Text != "")
            {
                int am = 0;
                bool res = int.TryParse((sender as TextBox).Text, out am);
                m_prihodModel.Prihod_amount = am;
            }
            else
                m_prihodModel.Prihod_amount = 0;
        }
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            string nameBt = (sender as RadButton).Name;

            if(par=="new")
            {
                m_prihodModel.Idnomenclatura = m_nomenklaturaModel.Idnomenklatura;
                m_prihodModel.Prihod_amount = int.Parse(PrihodTextBox.Text);
                m_prihodModel.Date_ = Option.CurrentDate;
                m_prihodModel.Insert();
            }
            else
            {
                m_prihodModel.Prihod_amount = int.Parse(PrihodTextBox.Text);
                m_prihodModel.Update();
            }

            MessageBox.Show("Данные сохранены");
            Option.CalculateBalance();
            if(nameBt == "SaveAndCloseBt") Close();

            PrihodModel = new PrihodModel();
            NomenklaturaModel = new NomenklaturaModel();
            PriceModel = new PriceModel();
            PrihodTextBox.Text = "";
        }
        #endregion
    }
}
