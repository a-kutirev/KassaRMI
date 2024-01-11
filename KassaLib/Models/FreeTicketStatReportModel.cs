using System;

namespace KassaLib.Models
{
    public class FreeTicketStatReportModel
    {
        #region Members
        private int m_idFreeTicketStat;
        private int m_amount;
        private int m_idexposition;
        private int m_idcategory;
        private string m_expositionname;
        private string m_categoryname;
        private DateTime m_FreeTicketStatDate;

        public int IdFreeTicketStat { get => m_idFreeTicketStat; set => m_idFreeTicketStat = value; }
        public int Amount { get => m_amount; set => m_amount = value; }
        public int Idexposition { get => m_idexposition; set => m_idexposition = value; }
        public int Idcategory { get => m_idcategory; set => m_idcategory = value; }
        public string Expositionname { get => m_expositionname; set => m_expositionname = value; }
        public string Categoryname { get => m_categoryname; set => m_categoryname = value; }
        public DateTime FreeTicketStatDate { get => m_FreeTicketStatDate; set => m_FreeTicketStatDate = value; }
        #endregion

        #region Ctor
        public FreeTicketStatReportModel()
        {

        }
        #endregion
    }
}
