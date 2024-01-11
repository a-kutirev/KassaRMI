using System;
using System.ComponentModel;
using System.Data;

namespace KassaLib.Models
{
    public class NewTicketsModel : INotifyPropertyChanged
    {
        #region Members
        private int m_idtickets = -1;
        private DateTime m_data = Option.CurrentDate;
        private string m_liter_tail = "";
        private int m_num_tail = 0;
        private int m_amount = 0;
        private int m_usenewnumeration = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Idtickets
        {
            get => m_idtickets;
            set
            {
                m_idtickets = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idtickets"));
            }
        }
        public DateTime Data
        {
            get => m_data;
            set
            {
                m_data = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Data"));
            }
        }
        public string Liter_tail
        {
            get => m_liter_tail;
            set
            {
                m_liter_tail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Liter_tail"));
            }
        }
        public int Num_tail
        {
            get => m_num_tail;
            set
            {
                m_num_tail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Num_tail"));
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

        public int Usenewnumeration
        {
            get => m_usenewnumeration;
            set
            {
                m_usenewnumeration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Usenewnumeration"));
            }
        }
        #endregion

        #region Constructor
        public NewTicketsModel() { }


        public NewTicketsModel(int id)
        {
            string sql = $"select * from newtickets where idtickets = {id}";
            DataTable dt = DBWrapper.Select(sql);
            if (dt.Rows.Count > 0)
            {
                m_idtickets = int.Parse(dt.Rows[0]["idtickets"].ToString());
                m_data = DateTime.Parse(dt.Rows[0]["data"].ToString());
                m_liter_tail = dt.Rows[0]["liter_tail"].ToString();
                m_num_tail = int.Parse(dt.Rows[0]["num_tail"].ToString());
                m_amount = int.Parse(dt.Rows[0]["amount"].ToString());
                m_usenewnumeration = int.Parse(dt.Rows[0]["usenewnumeration"].ToString());
            }
        }
        #endregion

        #region Insert
        public int Insert()
        {
            string sql = $"insert into newtickets(data, liter_tail, num_tail, amount, usenewnumeration) " +
                $"values('{m_data.ToString("yyyy-MM-dd")}', '{m_liter_tail}', {m_num_tail}, {m_amount}, {m_usenewnumeration})";
            m_idtickets = DBWrapper.Execute(sql);

            return m_idtickets;
        }
        #endregion

        #region Update
        public void Update()
        {
            string sql = $"update newtickets set " +
                $"data = '{m_data.ToString("yyyy-MM-dd")}', " +
                $"liter_tail = '{m_liter_tail}', " +
                $"num_tail = {m_num_tail}, " +
                $"amount = {m_amount}, " +
                $"usenewnumeration = {m_usenewnumeration} " +
                $"where idtickets = {m_idtickets}";
            DBWrapper.Execute(sql);
        }
        #endregion
    }
}
