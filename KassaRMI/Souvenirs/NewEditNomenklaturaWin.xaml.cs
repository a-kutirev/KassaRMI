using KassaLib;
using KassaLib.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

using Telerik.Windows.Controls;

namespace KassaRMI.Souvenirs
{
    /// <summary>
    /// Interaction logic for NewEditNomenklaturaWin.xaml
    /// </summary>
    public partial class NewEditNomenklaturaWin: INotifyPropertyChanged
    {
        #region Members
        public event PropertyChangedEventHandler PropertyChanged;

        private NomenklaturaModel m_nomenklaturaModel;
        private PriceModel m_priceModel;
        private string par;
        private int id;

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
        public NewEditNomenklaturaWin(string param, int id)
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            this.par = param;
            this.id = id;

            InitializeComponent();
            this.DataContext = this;
            DialogResult = false;

            switch(param)
            {
                case "new":
                    Header = "Новый товар";
                    NomenklaturaModel = new NomenklaturaModel();
                    PriceModel = new PriceModel();
                    break;
                case "edit":
                    Header = "Редактирование";
                    NomenklaturaModel = new NomenklaturaModel(id);
                    PriceModel = new PriceModel(DateTime.Now, id);
                    PriceTextBox.IsReadOnly = true;
                    PriceTextBox.Background = Brushes.LightGray;
                    break;
            }
            
        }
        #endregion

        #region Events
        private void SaveAddBt_Click(object sender, RoutedEventArgs e)
        {
            string nameBt = (sender as RadButton).Name;

            if(par == "new")
            {
                int id = m_nomenklaturaModel.Insert();
                m_priceModel.Idnomenclatura = id;
                m_priceModel.Insert();
            }
            else
            {
                m_nomenklaturaModel.Update();
            }

            MessageBox.Show("Данные сохранены");
            Close();
        }
        private void PriceLabelBt_Click(object sender, RoutedEventArgs e)
        {
            PriceLabelWindow win = new PriceLabelWindow(m_nomenklaturaModel);
            win.ShowDialog();
        }
        #endregion
    }
}
