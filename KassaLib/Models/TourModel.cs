using System.ComponentModel;
using System.Data;

namespace KassaLib.Models
{
    public class TourModel : INotifyPropertyChanged
    {
        #region Members
        private int m_idtours = -1;
        private string m_name = "";

        public int Idtours
        {
            get => m_idtours;
            set
            {
                m_idtours = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idtours"));
            }
        }
        public string Name
        {
            get => m_name;
            set
            {
                m_name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public TourModel()
        {

        }
        public TourModel(int id)
        {
            string sql = $"select * from tours where idtours = {id}";
            DataTable tmp = DBWrapper.Select(sql);

            if (tmp.Rows.Count > 0)
            {
                m_idtours = id;
                m_name = tmp.Rows[0]["name"].ToString();
            }
        }
        #endregion

        #region Insert
        public void Insert()
        {
            string sql = $"insert into tours(name) values ('{m_name}')";
            Idtours = DBWrapper.Execute(sql);
        }
        #endregion

        #region Update
        public void Update()
        {
            string sql = $"update tours set " +
                $"name = '{m_name}' " +
                $"where idtours = {m_idtours}";
            DBWrapper.Execute(sql);
        }
        #endregion
    }
}
