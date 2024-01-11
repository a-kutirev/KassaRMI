using KassaLib;
using System;
using System.Data;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for TablePrihodWindow.xaml
    /// </summary>
    public partial class TablePrihodWindow
    {
        #region Members
        #endregion

        #region Ctor
        public TablePrihodWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            UpdateDataGrid();
        }

        #endregion

        #region Events
        private void CloseBt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateDataGrid()
        {
            string sql = "select * from newtickets order by data desc";
            DataTable dt = DBWrapper.Select(sql);

            DataTable tmp = new DataTable();
            tmp.Columns.Add("id", typeof(int));
            tmp.Columns.Add("date", typeof(DateTime));
            tmp.Columns.Add("code", typeof(string));
            tmp.Columns.Add("amount", typeof(int));
            tmp.Columns.Add("newNum", typeof(Boolean));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dataRow = tmp.NewRow();

                dataRow["id"] = (int)dt.Rows[i]["idtickets"];
                dataRow["date"] = DateTime.Parse(dt.Rows[i]["data"].ToString());
                string part_1 = dt.Rows[i]["liter_tail"].ToString();
                string part_2 = ((int)dt.Rows[i]["num_tail"]).ToString("D6");
                dataRow["code"] = $"{part_1}{part_2}";
                dataRow["amount"] = (int)dt.Rows[i]["amount"];
                dataRow["newNum"] = dt.Rows[i]["usenewnumeration"].ToString() == "1";

                tmp.Rows.Add(dataRow);
            }

            PrihodGridView.ItemsSource = null;
            PrihodGridView.ItemsSource = tmp.DefaultView;
        }
        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            int id_pr = (int)(sender as RadButton).CommandParameter;
            PrihodTicketWindow win = new PrihodTicketWindow("edit", id_pr);
            win.ShowDialog();

            UpdateDataGrid();
        }

        #endregion
    }
}
