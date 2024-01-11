using System;

namespace KassaLib.Models
{
    public class TicketsModel
    {
        #region Members
        private int m_idticket = -1;
        private DateTime m_selldate = Option.CurrentDate;
        private int m_idtours = -1;
        private int m_idcategory = -1;
        private int m_amount = 0;
        #endregion

        #region Constructor
        public TicketsModel()
        {

        }

        public int Idticket { get => m_idticket; set => m_idticket = value; }
        public DateTime Selldate { get => m_selldate; set => m_selldate = value; }
        public int Idtours { get => m_idtours; set => m_idtours = value; }
        public int Idcategory { get => m_idcategory; set => m_idcategory = value; }
        public int Amount { get => m_amount; set => m_amount = value; }
        #endregion

        #region Update
        public void Update()
        {
            string sql = $"update ticket set " +
                $"selldate = '{m_selldate.ToString("yyyy-MM-dd")}', " +
                $"idtours = {m_idtours}, " +
                $"idcategory = {m_idcategory}, " +
                $"amount = {m_amount} " +
                $"where idticket = {m_idticket}";
            DBWrapper.Execute(sql);
        }
        #endregion

        #region Insert

        public void Insert()
        {
            string sql = $"insert into ticket(selldate, idtours, idcategory, amount) values" +
                $"('{m_selldate.ToString("yyyy-MM-dd")}', {m_idtours}, {m_idcategory}, {m_amount})";

            m_idticket = DBWrapper.Execute(sql);
        }

        #endregion
    }
}
