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
    /// Interaction logic for TableCert.xaml
    /// </summary>
    public partial class TableCert
    {
        #region Members

        #endregion

        #region Ctor
        public TableCert()
        {
            InitializeComponent();
            LocalizationManager.Manager = new CustomLocalizationManager();

            RefreshTable();
        }
        #endregion

        #region Events
        private void RefreshTable()
        {
            string sql = "select * from certs;";
            DataTable dt = DBWrapper.Select(sql);

            TableCertView.ItemsSource = null;
            TableCertView.ItemsSource = dt.DefaultView;
        }
        #endregion

        private void EditCertBt_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as RadButton).CommandParameter;

            CertModel m = new CertModel(id);
            EditTableCert win = new EditTableCert(m);
            win.ShowDialog();
            if ((bool)win.DialogResult)
            {
                RefreshTable();
            }

        }
    }
}
