using System;
using System.ComponentModel;
using System.Data;

namespace KassaLib.Models
{
    public class PrihodModel : INotifyPropertyChanged
    {
        #region Members
        private int m_idprihod = 0;
        private int m_idnomenclatura = 0;
        private int m_prihod_amount = 0;
        private DateTime m_date_ = Option.CurrentDate;

        public int Idprihod
        {
            get => m_idprihod;
            set
            {
                m_idprihod = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idprihod"));
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
        public int Prihod_amount
        {
            get => m_prihod_amount;
            set
            {
                m_prihod_amount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Prihod_amount"));
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

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public PrihodModel()
        {

        }
        public PrihodModel(int id)
        {
            string sql = $"SELECT * FROM prihod where idprihod = {id}";
            DataTable tmp = DBWrapper.Select(sql);

            if(tmp.Rows.Count > 0)
            {
                m_idprihod = id;
                m_idnomenclatura = (int)tmp.Rows[0]["idnomenclatura"];
                m_prihod_amount = (int)tmp.Rows[0]["prihod_amount"];
                m_date_ = DateTime.Parse(tmp.Rows[0]["date_"].ToString());
            }
        }
        #endregion

        #region Insert
        public int Insert()
        {
            int result = 0;
            string sql = $"insert into prihod(idnomenclatura, prihod_amount, date_) " +
                $"values({m_idnomenclatura}, {m_prihod_amount}, '{m_date_.ToString("yyyy-MM-dd")}')";
            result = DBWrapper.Execute(sql);
            m_idprihod = result;
            return result;
        }
        #endregion

        #region Update
        public void Update()
        {
            string sql =$"update prihod set " +
                        $"idnomenclatura = {m_idnomenclatura}, " +
                        $"prihod_amount = {m_prihod_amount}, " +
                        $"date_ = '{m_date_.ToString("yyyy-MM-dd")}' " +
                        $"where idprihod = {m_idprihod}";

            DBWrapper.Execute(sql);
        }
        #endregion
    }
}
