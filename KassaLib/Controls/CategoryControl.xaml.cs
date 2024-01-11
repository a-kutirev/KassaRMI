using KassaLib.Models;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KassaLib.Controls
{
    /// <summary>
    /// Логика взаимодействия для CategoryControl.xaml
    /// </summary>
    public partial class CategoryControl : UserControl, INotifyPropertyChanged
    {
        #region Members
        private CategoryModel m_model = new CategoryModel();
        private bool m_isActive;
        private bool loaded = false;
        private int m_Amount;

        public CategoryModel Model
        {
            get => m_model;
            set
            {
                m_model = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Model"));
            }
        }

        public bool IsActive
        {
            get => m_isActive;
            set
            {
                m_isActive = value;
                if (!m_isActive) AmountTextBox.Text = "";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsActive"));
            }
        }
        public int Amount
        {
            get => m_Amount;
            set
            {
                m_Amount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Amount"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Ctor
        public CategoryControl()
        {
            InitializeComponent();
            this.DataContext = this;

            loaded = true;
            IsActive = false;
        }        
        #endregion

        #region Events
        private void AmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void EnebleInput_Checked(object sender, RoutedEventArgs e)
        {
            if (!loaded) return;
            AmountTextBox.IsEnabled = (bool)(sender as CheckBox).IsChecked;

            if (!AmountTextBox.IsEnabled)
            {
                AmountTextBox.Text = "0";
            }
            else
            {
                AmountTextBox.Text = "";
                AmountTextBox.Focus();
            }
        }
        #endregion
    }
}
