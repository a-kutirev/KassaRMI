using System;
using System.Data;

namespace KassaLib.Models
{
    public class free_ticket_sellModel
    {
        #region Memers
        private int m_idfree_ticket_sell = -1;
        private DateTime m_date_ = Option.CurrentDate;
        private int m_amount = 0;
        private int m_amount_free = 0;
        private int m_amount_notfree = 0;
        #endregion

        #region Constructor
        public free_ticket_sellModel() { }

        public free_ticket_sellModel(int id)
        {
            string sql = $"select * from free_ticket_sell where idfree_ticket_sell = {id}";
            DataTable dt = DBWrapper.Select(sql);
            if (dt.Rows.Count > 0)
            {
                m_idfree_ticket_sell = int.Parse(dt.Rows[0]["idfree_ticket_sell"].ToString());
                m_date_ = DateTime.Parse(dt.Rows[0]["date_"].ToString());
                m_amount = int.Parse(dt.Rows[0]["amount"].ToString());
                m_amount_free = int.Parse(dt.Rows[0]["amount_free"].ToString());
                m_amount_notfree = int.Parse(dt.Rows[0]["amount_notfree"].ToString());
            }
        }

        public int Idfree_ticket_sell { get => m_idfree_ticket_sell; set => m_idfree_ticket_sell = value; }
        public DateTime Date_ { get => m_date_; set => m_date_ = value; }
        public int Amount { get => m_amount; set => m_amount = value; }
        public int Amount_free { get => m_amount_free; set => m_amount_free = value; }
        public int Amount_notfree { get => m_amount_notfree; set => m_amount_notfree = value; }
        #endregion

        #region Insert
        public int Insert()
        {
            string sql = $"insert into free_ticket_sell(date_, amount, amount_free, amount_notfree) values " +
                $"('{Date_.ToString("yyyy-MM-dd")}', {Amount}, {m_amount_free}, {m_amount_notfree})";
            Idfree_ticket_sell = DBWrapper.Execute(sql);

            return Idfree_ticket_sell;
        }
        #endregion

        #region Update
        public void Update()
        {
            string sql = $"update free_ticket_sell set " +
                $"date_ = '{Date_.ToString("yyyy-MM-dd")}'," +
                $"amount = {Amount}, " +
                $"amount_free = {Amount_free}, " +
                $"amount_notfree = {Amount_notfree} " +
                $"where idfree_ticket_sell = {Idfree_ticket_sell}";
            DBWrapper.Execute(sql);
        }
        #endregion
    }
}
