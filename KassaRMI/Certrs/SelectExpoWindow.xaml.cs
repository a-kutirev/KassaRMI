using KassaLib;
using KassaLib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Telerik.Windows.Controls;

namespace KassaRMI.Certrs
{
    /// <summary>
    /// Interaction logic for SelectExpoWindow.xaml
    /// </summary>
    public partial class SelectExpoWindow
    {
        public int IdExpo = -1;        

        public SelectExpoWindow()
        {
            InitializeComponent();
            DialogResult = false;

            string sql = "SELECT min(idexposition) as idexposition, expositionname FROM kassa.exposition\r\ngroup by expositionname order by min(idexposition);";
            DataTable dt = DBWrapper.Select(sql);

            ExpositionGrid.ItemsSource = dt.DefaultView;
        }

        private void ExpositionGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView drv = (sender as RadGridView).SelectedItem as DataRowView;
            DataRow dr = drv.Row;

            IdExpo = (int)dr["idexposition"];
            DialogResult = true;
            Close();
        }
    }
}
