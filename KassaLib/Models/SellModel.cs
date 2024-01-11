using System;
using System.ComponentModel;
using System.Data;

namespace KassaLib.Models
{
    public class SellModel : INotifyPropertyChanged
    {
        #region Members
        private int m_idsell = 0;
        private int m_idnomenclatura = 0;
        private int m_amount = 0;
        private int m_spisanie = 0;
        private float m_price_total = 0;
        private string m_cash_card = "cash";
        private string m_comment = "";
        private DateTime m_date_ = Option.CurrentDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Idsell
        {
            get => m_idsell;
            set
            {
                m_idsell = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idsell"));
            }
        }
        public int Idnomenclatura
        {
            get => m_idnomenclatura;
            set
            {
                m_idnomenclatura = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idnomenclatura"));
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
        public float Price_total
        {
            get => m_price_total;
            set
            {
                m_price_total = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price_total"));
            }
        }
        public string Cash_card
        {
            get => m_cash_card;
            set
            {
                m_cash_card = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Cash_card"));
            }
        }
        public DateTime Date_
        {
            get => m_date_;
            set
            {
                m_date_ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
            }
        }

        public int Spisanie
        {
            get => m_spisanie;
            set
            {
                m_spisanie = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Spisanie"));
            }
        }
        public string Comment
        {
            get => m_comment;
            set
            {
                m_comment = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Comment"));
            }
        }
        #endregion

        #region Constructor
        public SellModel()
        {

        }
        public SellModel(int id)
        {
            string sql = $"select * from sell where idsell = {id}";
            DataTable tmp = DBWrapper.Select(sql);
            if (tmp.Rows.Count > 0)
            {
                m_idsell = int.Parse(tmp.Rows[0]["idsell"].ToString());
                m_idnomenclatura = int.Parse(tmp.Rows[0]["idnomenclatura"].ToString());
                m_amount = int.Parse(tmp.Rows[0]["amount"].ToString());
                m_price_total = float.Parse(tmp.Rows[0]["price_total"].ToString());
                m_cash_card = tmp.Rows[0]["cash_card"].ToString();
                m_date_ = DateTime.Parse(tmp.Rows[0]["date_"].ToString());
                m_spisanie = int.Parse(tmp.Rows[0]["spisanie"].ToString());
                m_comment = tmp.Rows[0]["comment"].ToString();
            }
        }
        #endregion

        #region Insert
        public int Insert()
        {
            int result = 0;

            string sql = $"insert into sell(idnomenclatura, amount, date_, price_total, cash_card, spisanie, comment) " +
                $"values({m_idnomenclatura}, {m_amount} , '{m_date_.ToString("yyyy-MM-dd")}', " +
                $"{m_price_total.ToString(System.Globalization.CultureInfo.InvariantCulture)}, '{m_cash_card}', " +
                $"{m_spisanie}, '{m_comment}')";
            result = DBWrapper.Execute(sql);
            return result;
        }
        #endregion

        #region Update
        public void Update()
        {
            string sql =
                $"update sell set "+
                $"idnomenclatura = {m_idnomenclatura}, "+
                $"amount = {m_amount}, "+
                $"date_ = '{m_date_.ToString("yyyy-MM-dd")}', "+
                $"price_total = {m_price_total.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
                $"cash_card = '{m_cash_card}', "+
                $"spisanie = {m_spisanie}, "+
                $"comment = '{m_comment}' "+
                $"WHERE idsell = {m_idsell}";

            DBWrapper.Execute(sql);
        }
        #endregion
    }
}
