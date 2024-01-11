using System;
using System.ComponentModel;
using System.Data;

namespace KassaLib.Models
{
    public class PriceModel : INotifyPropertyChanged
    {
        #region Members
        private int m_idprice = -1;
        private int m_idnomenclatura = -1;
        private float m_price = 0;
        private DateTime m_date_ = Option.CurrentDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Idprice
        {
            get => m_idprice;
            set
            {
                m_idprice = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idprice"));
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
        public float Price
        {
            get => m_price;
            set
            {
                m_price = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price"));
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
        #endregion

        #region Constructor
        public PriceModel()
        {

        }
        public PriceModel(DateTime date, int idnom)
        {
            string sql = $"SELECT * FROM kassa.price where idnomenclatura = {idnom}";
            DataTable dt = DBWrapper.Select(sql);
            if (dt.Rows.Count > 0)
            {
                Idprice = int.Parse(dt.Rows[0]["idprice"].ToString());
                Idnomenclatura = idnom;
                Date_ = DateTime.Parse(dt.Rows[0]["date_"].ToString());
                Price = float.Parse(dt.Rows[0]["price"].ToString());
            }
        }
        #endregion

        #region Insert
        public int Insert()
        {
            int result = 0;

            string sql = $"insert into price(idnomenclatura, price, date_)" +
                $"values({m_idnomenclatura}, {m_price.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
                $"'{m_date_.ToString("yyyy-MM-dd")}')";

            result = DBWrapper.Execute(sql);

            return result;
        }
        #endregion

        #region Update
        #endregion
    }
}
