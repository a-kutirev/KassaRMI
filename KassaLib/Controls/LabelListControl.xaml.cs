using KassaLib.Models;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KassaLib.Controls
{
    /// <summary>
    /// Логика взаимодействия для LabelListControl.xaml
    /// </summary>
    public partial class LabelListControl : UserControl, INotifyPropertyChanged
    {
        #region Members

        public event EventHandler<EditPriceEventHandler> OnEditPriceClick;

        private NomenklaturaModel m_model;
        private bool m_selected;
        private int m_amount;

        public NomenklaturaModel Model
        {
            get => m_model;
            set
            {
                m_model = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Model"));
            }
        }

        public bool Selected
        {
            get => m_selected;
            set
            {
                m_selected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Selected"));
            }
        }

        public int Amount
        {
            get => m_amount;
            set
            {
                m_amount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Amount"));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Ctor
        public LabelListControl(NomenklaturaModel nm)
        {
            InitializeComponent();
            this.DataContext = this;

            Model = nm;         
            Selected = false;
            Amount = 1;
        }
        #endregion

        #region Methods
        public bool ContainText(string text)
        {
            string upperText = text.ToUpper();
            string compared = Model.Nomenklaturaname.ToUpper();

            return compared.Contains(upperText);
        }
        #endregion

        #region Events
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            PriceEditLibWindow win = new PriceEditLibWindow(m_model);
            win.ShowDialog();

            m_model = win.NomenklaturaModel;

            m_model.Update();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
    }

    public class EditPriceEventHandler : EventArgs
    {
        public NomenklaturaModel model;
    }
}
