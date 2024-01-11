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
    /// Interaction logic for SellSouvenirWindow.xaml
    /// </summary>
    public partial class SellSouvenirWindow: INotifyPropertyChanged
    {
        #region Members
        public event PropertyChangedEventHandler PropertyChanged;

        private NomenklaturaModel m_nomenklaturaModel;
        private PriceModel m_priceModel;
        private SellModel m_sellModel;
        private float m_totalPrice = 0;
        private DateTime m_dateCurrent = Option.CurrentDate;
        private bool loaded = false;

        private string par;
        private int id_s;

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
        public SellModel SellModel
        {
            get => m_sellModel;
            set
            {
                m_sellModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SellModel"));
            }
        }
        public float TotalPrice
        {
            get => m_totalPrice;
            set
            {
                m_totalPrice = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TotalPrice"));
            }
        }      
        #endregion

        #region Ctor
        public SellSouvenirWindow(string param, int id)
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            par = param;
            id_s = id;

            InitializeComponent();
            this.DataContext = this;
            DialogResult = false;

            if (param == "new")
            {
                m_dateCurrent = Option.CurrentDate;
                SellModel = new SellModel();
                SellModel.Cash_card = "cash";                
                SetSpisanie(false);
            }
            else
            {
                SellModel = new SellModel(id);
                NomenklaturaModel = new NomenklaturaModel(SellModel.Idnomenclatura);
                PriceModel = new PriceModel(DateTime.Now, SellModel.Idnomenclatura);
                m_dateCurrent = SellModel.Date_;
                SpisanieCB.IsChecked = SellModel.Spisanie == 1;
                SellAmountTextBox.Text = SellModel.Amount.ToString();
                CommentTextBox.Text = SellModel.Comment;
                SaveBt.Visibility = Visibility.Hidden;
                SetSpisanie(SellModel.Spisanie == 1);
                if(m_sellModel.Cash_card == "cash")
                {
                    rb1.IsChecked = true;
                    rb2.IsChecked = false;
                }
                else
                {
                    rb1.IsChecked = false;
                    rb2.IsChecked = true;
                }
            }


            DateLabel.Content = m_dateCurrent.ToString("dd MMMM yyyy") + " г.";            
        }        
        #endregion

        #region Events
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_totalPrice = 0;
            int counter = 0;

            if ((sender as TextBox).Text == "")
            {
                SellModel.Price_total = m_totalPrice;
                return;
            }

            bool res = int.TryParse((sender as TextBox).Text, out counter);
            if (res)
            {
                m_totalPrice = counter * PriceModel.Price;
                SellModel.Amount = counter;
                SellModel.Price_total = m_totalPrice;
            }
            else
            {
                m_totalPrice = 0;
            }
        }
        private void rb1_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetSpisanie((bool)(sender as CheckBox).IsChecked);
        }
        private void SetSpisanie(bool spis)
        {
            m_sellModel.Spisanie = spis ? 1 : 0;

            if (spis)
            {
                c1.Visibility = Visibility.Hidden;
                c2.Visibility = Visibility.Hidden;
                c3.Visibility = Visibility.Collapsed;
                c4.Visibility = Visibility.Visible;
            }
            else
            {
                c1.Visibility = Visibility.Visible;
                c2.Visibility = Visibility.Visible;
                c3.Visibility = Visibility.Visible;
                c4.Visibility = Visibility.Collapsed;
            }
        }
        private void SelectTovarBt_Click(object sender, RoutedEventArgs e)
        {
            SelectTovarWindow win = new SelectTovarWindow();
            if ((bool)win.ShowDialog())
            {
                NomenklaturaModel = win.Model;
                PriceModel = new PriceModel(m_dateCurrent, NomenklaturaModel.Idnomenklatura);

                

                SellModel.Idnomenclatura = NomenklaturaModel.Idnomenklatura;
                SellModel.Date_ = m_dateCurrent;

                SellAmountTextBox.Focus();
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            string nameBt = (sender as RadButton).Name;

            if (m_sellModel.Amount == 0)
            {

                MessageBoxResult res =
                    MessageBox.Show(
                    "Вы не указазли количество, либо оно равно 0.\n Вы уверены что хотите сохранить данные?",
                    "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                if(res == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            m_sellModel.Cash_card = (bool)rb1.IsChecked ? "cash" : "card";
            m_sellModel.Comment = CommentTextBox.Text;

            if (par=="new")
            {
                m_sellModel.Insert();
            }
            else
            {
                m_sellModel.Update();
            }

            MessageBox.Show("Данные сохранены");
            Option.CalculateBalance();
            if (nameBt == "SaveAndCloseBt")Close();

            NomenklaturaModel = new NomenklaturaModel();
            SellModel = new SellModel();
            PriceModel = new PriceModel();
            SellAmountTextBox.Text = "";

            SellModel.Cash_card = "cash";
            SetSpisanie(false);
        }
    }
    #endregion
}
