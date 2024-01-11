using System;

namespace KassaLib.Models
{
    public class OptionsModel
    {
        private int m_idOption = -1;
        private string m_option_name = "";
        private DateTime m_option_date = DateTime.Now;
        private string m_option_val = "";

        public int IdOption { get => m_idOption; set => m_idOption = value; }
        public string Option_name { get => m_option_name; set => m_option_name = value; }
        public DateTime Option_date { get => m_option_date; set => m_option_date = value; }
        public string Option_val { get => m_option_val; set => m_option_val = value; }

        public OptionsModel(string name, string value, DateTime date)
        {
            m_option_name=name;
            m_option_date=date;
            m_option_val=value;

            string sql =
                    $"INSERT INTO option (option_name, option_date, option_val) "+
                    $"VALUES('{m_option_name}', '{m_option_date.ToString("yyyy-MM-dd")}', '{m_option_val}'); ";

            m_idOption = DBWrapper.Execute(sql);
        }
        public OptionsModel() { }
    }
}
