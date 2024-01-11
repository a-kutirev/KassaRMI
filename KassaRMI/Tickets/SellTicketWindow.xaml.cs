using KassaLib;
using KassaLib.Models;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for SellTicketWindow.xaml
    /// </summary>
    public partial class SellTicketWindow: INotifyPropertyChanged
    {
        #region Members
        public event PropertyChangedEventHandler PropertyChanged;
        private string par;
        private int id_s;

        private free_ticket_sellModel m_free_Ticket_SellModel;

        public free_ticket_sellModel Free_Ticket_SellModel
        {
            get => m_free_Ticket_SellModel;
            set
            {
                m_free_Ticket_SellModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Free_Ticket_SellModel"));
            }
        }
        #endregion

        #region Ctor
        public SellTicketWindow(DateTime currentDate, string v, int v1)
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            par = v;
            id_s = v1;

            InitializeComponent();
            this.DataContext = this;

            DialogResult = false;

            if(par == "new")
            {
                m_free_Ticket_SellModel = new free_ticket_sellModel();
            }
            else
            {
                m_free_Ticket_SellModel = new free_ticket_sellModel(id_s);
            }

            DateLabel.Content = $"{m_free_Ticket_SellModel.Date_.ToString("dd MMMM yy")} г.";


            /// TODO: Неверный расчет номера билета
            string sql = $"select * from newtickets where data < '{m_free_Ticket_SellModel.Date_.ToString("yyyy-MM-dd")}' order by data desc";
            DataTable tmp = DBWrapper.Select(sql);
            if (tmp.Rows.Count > 0)
            {
                string lit = tmp.Rows[0]["liter_tail"].ToString();
                int startnum = int.Parse(tmp.Rows[0]["num_tail"].ToString());
                DateTime dtime = DateTime.Parse(tmp.Rows[0]["data"].ToString());

                string allSells = $"select * from free_ticket_sell where date_ >= '{dtime.ToString("yyyy-MM-dd")}' and date_ <= '{Option.CurrentDate.ToString("yyyy-MM-dd")}'";
                tmp = DBWrapper.Select(allSells);
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    int amn = int.Parse(tmp.Rows[i]["amount"].ToString());
                    startnum += amn;
                }

                TicketNumTxt.Text = $"{lit}{startnum.ToString("D6")}";
            }            
        }
        #endregion

        #region Events
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            if(par=="new")
            {
                string sql = $"select * from free_ticket_sell where date_ = '{m_free_Ticket_SellModel.Date_.ToString("yyyy-MM-dd")}'";
                DataTable dt = DBWrapper.Select(sql);

                if(dt.Rows.Count > 0)
                {
                    if (MessageBox.Show("На данный день уже внесены данные о проданных билетах. Внести новые данные?", "Инофрмация",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        sql = $"delete from free_ticket_sell where date_ = '{m_free_Ticket_SellModel.Date_.ToString("yyyy-MM-dd")}'";
                        DBWrapper.Execute(sql);
                        m_free_Ticket_SellModel.Insert();
                    }
                    else
                    {
                        Close();
                    }
                }
                else
                    m_free_Ticket_SellModel.Insert();
            }
            else
            {
                m_free_Ticket_SellModel.Update();
            }
            MessageBox.Show("Данные сохранены");
            Close();
        }
        #endregion
    }
}
