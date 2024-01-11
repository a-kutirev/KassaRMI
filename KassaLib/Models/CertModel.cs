using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaLib.Models
{
    public class CertModel : INotifyPropertyChanged
    {

        #region Init
        private int m_id = -1;
        private int m_idexposition = 70;
        private string m_nomer_cert = "";
        private string m_nominal = "";
        private bool m_exqurs = false;
        private DateTime m_datavydachi = Option.CurrentDate;
        private bool m_isactive = false;
        private DateTime? m_datapogasheniya = null;


        public int Id
        {
            get => m_id;
            set
            {
                m_id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id"));
            }
        }

        public string Nomer_cert
        {
            get => m_nomer_cert;
            set
            {
                m_nomer_cert = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Nomer_cert"));
            }
        }

        public string Nominal
        {
            get => m_nominal;
            set
            {
                m_nominal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Nominal"));
            }
        }

        public bool Exqurs
        {
            get => m_exqurs; set
            {
                m_exqurs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Exqurs"));
            }
        }

        public DateTime Datavydachi
        {
            get => m_datavydachi; set
            {
                m_datavydachi = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Datavydachi"));
            }
        }

        public bool Isactive
        {
            get => m_isactive; set
            {
                m_isactive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Isactive"));
            }
        }

        public DateTime? Datapogasheniya
        {
            get => m_datapogasheniya; set
            {
                m_datapogasheniya = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Datapogasheniya"));
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

        public event PropertyChangedEventHandler PropertyChanged;


        #endregion


        #region Ctor
        public CertModel()
        {

        }

        public CertModel(int id)
        {
            string sql = $"SELECT * FROM certs where id = {id}";
            DataTable tmp = DBWrapper.Select(sql);
            if (tmp.Rows.Count > 0)
            {
                m_id = id;
                m_nomer_cert = tmp.Rows[0]["nomer_cert"].ToString();
                m_nominal = tmp.Rows[0]["nominal"].ToString();
                m_exqurs = ((sbyte)tmp.Rows[0]["exqurs"]) == (sbyte)1;
                m_datavydachi = DateTime.Parse(tmp.Rows[0]["datavydachi"].ToString());
                m_isactive = ((sbyte)tmp.Rows[0]["isactive"]) == (sbyte)1;
                m_idexposition = (int)tmp.Rows[0]["idexposition"];
                try
                {
                    // У нас дата погашения еще не введена и там пустая строка, из за этого ошибка была
                    // Сейчас если у нас ошибка, то просто нулевое значение будет
                    m_datapogasheniya = DateTime.Parse(tmp.Rows[0]["datapogasheniya"].ToString());
                }
                catch
                {
                    m_datapogasheniya = null;
                };
            }
        }

        public CertModel(string nomcert)
        {
            string sql = $"SELECT * FROM certs where nomer_cert = '{nomcert}'";
            DataTable tmp = DBWrapper.Select(sql);
            if (tmp.Rows.Count > 0)
            {
                m_id = int.Parse(tmp.Rows[0]["id"].ToString());
                m_nomer_cert = tmp.Rows[0]["nomer_cert"].ToString();
                m_nominal = tmp.Rows[0]["nominal"].ToString();
                m_exqurs = ((sbyte)tmp.Rows[0]["exqurs"]) == (sbyte)1;
                m_datavydachi = DateTime.Parse(tmp.Rows[0]["datavydachi"].ToString());
                m_isactive = ((sbyte)tmp.Rows[0]["isactive"]) == (sbyte)1;
                m_idexposition = (int)tmp.Rows[0]["idexposition"];
                try
                {
                    m_datapogasheniya = DateTime.Parse(tmp.Rows[0]["datapogasheniya"].ToString());
                }
                catch
                {
                    m_datapogasheniya = null;
                }
            }
        }
        #endregion


        #region Update

        public void Update()
        {
            string str_pog = m_datapogasheniya == null ? string.Empty : ((DateTime)m_datapogasheniya).ToString("yyyy-MM-dd");
            string sql = string.Empty;

            if(str_pog != string.Empty)
                sql =
                    $"UPDATE certs " +
                    $"SET " +
                    $"nomer_cert = '{m_nomer_cert}', " +
                    $"idexposition = {m_idexposition}, " +
                    $"nominal = '{m_nominal}', " +
                    $"exqurs = {(m_exqurs ? 1 : 0)}, " +
                    $"datavydachi = '{m_datavydachi.ToString("yyyy-MM-dd")}', " +
                    $"isactive = {(m_isactive ? 1 : 0)}, " +
                    $"datapogasheniya= '{ str_pog }' " +
                    $"WHERE id = {m_id} ";
            else
                sql =
                    $"UPDATE certs " +
                    $"SET " +
                    $"nomer_cert = '{m_nomer_cert}', " +
                    $"idexposition = {m_idexposition}, " +
                    $"nominal = '{m_nominal}', " +
                    $"exqurs = {(m_exqurs ? 1 : 0)}, " +
                    $"datavydachi = '{m_datavydachi.ToString("yyyy-MM-dd")}', " +
                    $"isactive = {(m_isactive ? 1 : 0)}, " +
                    $"datapogasheniya= NULL " +  // Если пустая строка - то используем значение NULL, из за этого вылет
                    $"WHERE id = {m_id} ";

            DBWrapper.Execute(sql);
        }
        #endregion


        #region Insert
        public int Insert()
        {
            int result = 0;
            string sql = $"insert into certs(" +
                $"nomer_cert, " +
                $"nominal, " +
                $"exqurs, " +
                $"datavydachi, " +
                $"idexposition, " +
                $"isactive) " +
                $"values(" +
                $"'{m_nomer_cert}', " +
                $"'{m_nominal}', " +
                $"{(m_exqurs ? 1 : 0)}, " +
                $"'{m_datavydachi.ToString("yyyy-MM-dd")}', " +
                $"{m_idexposition}, " +
                $"{(m_isactive ? 1 : 0)}) ";
            result = DBWrapper.Execute(sql);

            return m_id = result;
        }
        #endregion



    }
}
