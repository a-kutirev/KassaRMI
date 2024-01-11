using System;
using System.ComponentModel;
using System.Data;

namespace KassaLib.Models
{
    public class FreeTicketStatModel : INotifyPropertyChanged
    {
        #region Members
        private int m_idfreeticketstat = -1;
        private int m_idcategory = -1;
        private int m_idexposition = -1;
        private int m_amount = -1;
        private bool m_isnotfree = false;
        private DateTime m_FreeTicketStatDate = Option.CurrentDate;

        public int Idfreeticketstat
        {
            get => m_idfreeticketstat;
            set
            {
                m_idfreeticketstat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idfreeticketstat"));
            }
        }
        public int Idcategory
        {
            get => m_idcategory;
            set
            {
                m_idcategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idcategory"));
            }
        }
        public int Idexposition
        {
            get => m_idexposition;
            set
            {
                m_idexposition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idexposition"));
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
        public bool Isnotfree
        {
            get => m_isnotfree;
            set
            {
                m_isnotfree = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Isnotfree"));
            }
        }
        public DateTime FreeTicketStatDate
        {
            get => m_FreeTicketStatDate; set
            {
                m_FreeTicketStatDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FreeTicketStatDate"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public FreeTicketStatModel()
        {

        }
        public FreeTicketStatModel(int id)
        {
            string sql = $"SELECT * FROM FreeTicketStat where idFreeTicketStat = {id}";
            DataTable tmp = DBWrapper.Select(sql);

            if(tmp.Rows.Count > 0)
            {
                m_idfreeticketstat = (int)tmp.Rows[0]["idFreeTicketStat"];
                m_idcategory = (int)tmp.Rows[0]["idcategory"];
                m_idexposition = (int)tmp.Rows[0]["idexposition"]; 
                m_amount = (int)tmp.Rows[0]["amount"];
                m_FreeTicketStatDate = DateTime.Parse(tmp.Rows[0]["FreeTicketStatDate"].ToString());
                m_isnotfree = int.Parse(tmp.Rows[0]["isnotfree"].ToString()) == 1;
            }
        }

        #endregion

        #region Insert
        public int Insert()
        {
            int result = 0;
            string sql = $"insert into FreeTicketStat(" +
                $"idcategory, " +
                $"idexposition, " +
                $"amount, " +
                $"FreeTicketStatDate, " +
                $"isnotfree) " +
                $"values(" +
                $"{m_idcategory}, " +
                $"{m_idexposition}, " +
                $"{m_amount}, " +
                $"'{m_FreeTicketStatDate.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                $"{m_isnotfree})";
            result = DBWrapper.Execute(sql);
            m_idfreeticketstat = result;
            return result;
        }
        #endregion

        #region Update
        public void Update()
        {
            string sql =
                $"update FreeTicketStat set " +
                $"idcategory = {m_idcategory}, " +
                $"idexposition = {m_idexposition}, " +
                $"amount = {m_amount}, " +
                $"FreeTicketStatDate = '{m_FreeTicketStatDate.ToString("yyyy-MM-dd")}', " +
                $"isnotfree = {m_isnotfree} " +
                $"where idFreeTicketStat = {m_idfreeticketstat} ";

            DBWrapper.Execute(sql);

        }
        #endregion
    }
}
