using System.ComponentModel;
using System.Data;

namespace KassaLib.Models
{
    public class NomenklaturaModel : INotifyPropertyChanged
    {
        #region Members
        private int m_idnomenklatura;
        private int m_idcategory;
        private int m_balance;
        private string m_nomenklaturaname;
        private string m_string_1 = "";
        private string m_string_2 = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public int Idnomenklatura
        {
            get => m_idnomenklatura;
            set
            {
                m_idnomenklatura = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idnomenklatura"));
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
        public string Nomenklaturaname
        {
            get => m_nomenklaturaname;
            set
            {
                m_nomenklaturaname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Nomenklaturaname"));
            }
        }
        public int Balance
        {
            get => m_balance;
            set
            {
                m_balance = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Balance"));
            }
        }

        public string String_1
        {
            get => m_string_1;
            set
            {
                m_string_1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("String_1"));
            }
        }
        public string String_2
        {
            get => m_string_2;
            set
            {
                m_string_2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("String_2"));
            }
        }

        #endregion

        #region Constructor
        public NomenklaturaModel()
        {

        }
        public NomenklaturaModel(int id)
        {
            string sql = $"select * from nomenklatura where idnomenklatura = {id}";
            DataTable tmp = DBWrapper.Select(sql);
            if (tmp.Rows.Count > 0)
            {
                m_idnomenklatura = id;
                m_nomenklaturaname = tmp.Rows[0]["nomenklaturaname"].ToString();
                m_idcategory = int.Parse(tmp.Rows[0]["idcategory"].ToString());
                m_balance = int.Parse(tmp.Rows[0]["balance"].ToString());
                m_string_1 = tmp.Rows[0]["string_1"].ToString();
                m_string_2 = tmp.Rows[0]["string_2"].ToString();
            }
        }
        #endregion

        #region Insert
        public int Insert()
        {
            int result = 0;

            string sql = $"insert into nomenklatura(idcategory, nomenklaturaname, balance, string_1, string_2) " +
                $"values({m_idcategory}, '{m_nomenklaturaname}', {m_balance}, '{m_string_1}','{m_string_2}')";

            result = DBWrapper.Execute(sql);
            m_idnomenklatura = result;

            return result;
        }
        #endregion

        #region Update
        public void Update()
        {
            string sql = $"update nomenklatura set " +
                $"idcategory = {m_idcategory}," +
                $"nomenklaturaname = '{m_nomenklaturaname}', " +
                $"balance = {m_balance}, " +
                $"string_1 = '{m_string_1}', " +
                $"string_2 = '{m_string_2}' " +
                $"where idnomenklatura = {m_idnomenklatura}";
            DBWrapper.Execute(sql);
        }
        #endregion
    }
}
