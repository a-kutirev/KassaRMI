using System;
using System.ComponentModel;
using System.Data;

namespace KassaLib.Models
{
    public class ExpositionModel : INotifyPropertyChanged
    {
        #region Members
        private int m_idexposition = -1;
        private string m_expositionname = "";
        private bool m_expositionpersistent = false;
        private bool m_expositionvrem = false;
        private bool m_expositionzam = false;
        private int m_expositionzamexpo = -1;
        private DateTime m_expositionstart = Option.CurrentDate;
        private DateTime m_expositionend = Option.CurrentDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Idexposition
        {
            get => m_idexposition;
            set
            {
                m_idexposition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idexposition"));
            }
        }
        public string Expositionname
        {
            get => m_expositionname;
            set
            {
                m_expositionname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Expositionname"));
            }
        }
        public bool Expositionpersistent
        {
            get => m_expositionpersistent;
            set
            {
                m_expositionpersistent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Expositionpersistent"));
            }
        }
        public bool Expositionzam
        {
            get => m_expositionzam;
            set
            {
                m_expositionzam = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Expositionzam"));
            }
        }
        public int Expositionzamexpo
        {
            get => m_expositionzamexpo;
            set
            {
                m_expositionzamexpo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Expositionzamexpo"));
            }
        }
        public DateTime Expositionstart
        {
            get => m_expositionstart;
            set
            {
                m_expositionstart = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Expositionstart"));
            }
        }
        public DateTime Expositionend
        {
            get => m_expositionend;
            set
            {
                m_expositionend = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Expositionend"));
            }
        }
        public bool Expositionvrem
        {
            get => m_expositionvrem;
            set
            {
                m_expositionvrem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Expositionvrem"));
            }
        }
        #endregion

        #region Ctor
        public ExpositionModel()
        {

        }
        public ExpositionModel(int id)
        {
            string sql = $"SELECT * FROM exposition where idexposition = {id}";
            DataTable tmp = DBWrapper.Select(sql);
            if(tmp.Rows.Count > 0)
            {
                m_idexposition = id;
                m_expositionname = tmp.Rows[0]["expositionname"].ToString();
                m_expositionpersistent = ((sbyte)tmp.Rows[0]["expositionpersistent"]) == (sbyte)1;
                m_expositionzam = ((sbyte)tmp.Rows[0]["expositionzam"]) == (sbyte)1;
                m_expositionstart = DateTime.Parse(tmp.Rows[0]["expositionstart"].ToString());
                m_expositionend = DateTime.Parse(tmp.Rows[0]["expositionend"].ToString());
                m_expositionzamexpo = (int)tmp.Rows[0]["expositionzamexpo"];
            }
        }
        #endregion

        #region Update

        public void Update()
        {
            string sql =
                $"UPDATE exposition " +
                $"SET " +
                $"expositionname = '{m_expositionname}', " +
                $"expositionpersistent = {(m_expositionpersistent ? 1 : 0 )}, " +
                $"expositionzam = {(m_expositionzam ? 1 : 0)}, " +
                $"expositionstart = '{m_expositionstart.ToString("yyyy-MM-dd")}', " +
                $"expositionend = '{m_expositionend.ToString("yyyy-MM-dd")}', " +
                $"expositionzamexpo = {m_expositionzamexpo}, " +
                $"expositionvrem = 0 " +
                $"WHERE idexposition = {m_idexposition} ";

            DBWrapper.Execute(sql);
        }


        #endregion

        #region Insert
        public int Insert()
        {
            int result = 0;
            string sql = $"insert into exposition(" +
                $"expositionname, " +
                $"expositionpersistent, " +
                $"expositionzam, " +
                $"expositionstart, " +
                $"expositionend, " +
                $"expositionzamexpo) " +
                $"values(" +
                $"'{m_expositionname}', " +
                $"{(m_expositionpersistent ? 1 : 0)}, " +
                $"{(m_expositionzam ? 1 : 0)}, " +
                $"'{m_expositionstart.ToString("yyyy-MM-dd")}', " +
                $"'{m_expositionend.ToString("yyyy-MM-dd")}', " +
                $"{m_expositionzamexpo})";
            result = DBWrapper.Execute(sql);
            m_idexposition = result;
            return result;
        }
        #endregion
    }
}
