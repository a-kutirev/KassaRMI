using KassaLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

using Telerik.Windows.Controls;

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for TableExpoWindow.xaml
    /// </summary>
    public partial class TableExpoWindow
    {
        #region Members

        private DataTable source;

        #endregion

        #region Ctor
        public TableExpoWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            source = new DataTable();
            DataColumn col = new DataColumn("id", typeof(int));
            source.Columns.Add(col);
            col = new DataColumn("name", typeof(string));
            source.Columns.Add(col);
            col = new DataColumn("pers", typeof(bool));
            source.Columns.Add(col);
            col = new DataColumn("zam", typeof(bool));
            source.Columns.Add(col);
            col = new DataColumn("zamname", typeof(string));
            source.Columns.Add(col);
            col = new DataColumn("start", typeof(string));
            source.Columns.Add(col);
            col = new DataColumn("end", typeof(string));
            source.Columns.Add(col);

            RefreshTable();
        }
        #endregion

        #region Events
        private void RefreshTable()
        {
            string sql = "select * from exposition;";
            DataTable dt = DBWrapper.Select(sql);
            Dictionary<int, string> expoDict = new Dictionary<int, string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int id = (int)dt.Rows[i]["idexposition"];
                string name = dt.Rows[i]["expositionname"].ToString();
                expoDict.Add(id, name);
            }

            source.Rows.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = source.NewRow();

                dr["id"] = (int)dt.Rows[i]["idexposition"];
                dr["name"] = dt.Rows[i]["expositionname"].ToString();
                dr["pers"] = dt.Rows[i]["expositionpersistent"].ToString() == "1";
                dr["zam"] = dt.Rows[i]["expositionzam"].ToString() == "1";
                dr["zamname"] = (dt.Rows[i]["expositionzam"].ToString() == "1") ? expoDict[(int)dt.Rows[i]["expositionzamexpo"]] : "";
                dr["start"] = (dt.Rows[i]["expositionpersistent"].ToString() == "1") ? "" : ((DateTime)dt.Rows[i]["expositionstart"]).ToString("dd-MM-yyyy");
                dr["end"] = (dt.Rows[i]["expositionpersistent"].ToString() == "1") ? "" : ((DateTime)dt.Rows[i]["expositionend"]).ToString("dd-MM-yyyy");

                source.Rows.Add(dr);
            }

            ExpoGridView.ItemsSource = null;
            ExpoGridView.ItemsSource = source.DefaultView;
        }
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void EditBt_Click(object sender, RoutedEventArgs e)
        {
            string btName = (sender as RadButton).Name;
            NewEditExpoWindow win;

            if (btName == "NewExpoBt")
            {
                win = new NewEditExpoWindow("new", -1);
            }
            else
            {
                int id = (int)(sender as RadButton).CommandParameter;
                win = new NewEditExpoWindow("edit", id);
            }
            
            if((bool)win.ShowDialog())
            {
                RefreshTable();
            }
        }
        #endregion
    }
}
