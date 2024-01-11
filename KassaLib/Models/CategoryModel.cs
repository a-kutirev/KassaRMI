using System.ComponentModel;
using System.Data;

namespace KassaLib.Models
{
    public class CategoryModel : INotifyPropertyChanged
    {
        #region Members
        private int m_idcategory = 0;
        private string m_categoryname = "";

        public int Idcategory
        {
            get => m_idcategory;
            set
            {
                m_idcategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Idcategory"));
            }
        }
        public string Categoryname
        {
            get => m_categoryname;
            set
            {
                m_categoryname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Categoryname"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public CategoryModel()
        {

        }
        public CategoryModel(int id)
        {
            string sql = $"select * from category where idcategory = {id}";

            DataTable tmp = DBWrapper.Select(sql);
            if (tmp.Rows.Count > 0)
            {
                m_idcategory = id;
                m_categoryname = tmp.Rows[0]["categoryname"].ToString();
            }
        }
        #endregion

        #region Insert
        public void Insert()
        {
            string sql = $"insert into category(categoryname) values ('{m_categoryname}')";
            m_idcategory = DBWrapper.Execute(sql);
        }
        #endregion

        #region Update
        // TODO 

        #endregion
    }
}
