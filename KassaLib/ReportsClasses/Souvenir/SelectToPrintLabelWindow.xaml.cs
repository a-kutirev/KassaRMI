using KassaLib.Controls;
using KassaLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Telerik.Reporting;

namespace KassaLib.ReportsClasses.Souvenir
{
    /// <summary>
    /// Interaction logic for SelectToPrintLabelWindow.xaml
    /// </summary>
    public partial class SelectToPrintLabelWindow
    {
        #region Members
        //List<CheckBox> checked_list = new List<CheckBox>();

        List<LabelListControl> labelListControls = new List<LabelListControl>();
        #endregion

        #region Ctor
        public SelectToPrintLabelWindow()
        {
            InitializeComponent();

            string sql =
                "SELECT nomenklatura.idnomenklatura, nomenklatura.nomenklaturaname,price.price " +
                "FROM kassa.nomenklatura " +
                "inner join price on nomenklatura.idnomenklatura = price.idnomenclatura " +
                "where string_1<> '' or string_2<> '' ";

            DataTable tmp = DBWrapper.Select(sql);                        

            for (int i = 0; i < tmp.Rows.Count; i++)
            {
                int id = int.Parse(tmp.Rows[i]["idnomenklatura"].ToString());
                string n = tmp.Rows[i]["nomenklaturaname"].ToString();

                LabelListControl llc = new LabelListControl(new NomenklaturaModel(id));
                llc.OnEditPriceClick += Llc_OnEditPriceClick;
                labelListControls.Add(llc);
                NomListBox.Items.Add(llc);

                //CheckBox chk = new CheckBox();
                //chk.Content = n;
                //chk.CommandParameter = id;

                //checked_list.Add(chk);
                //NomListBox.Items.Add(chk);
            }            
        }
        #endregion

        #region Events
        private void Llc_OnEditPriceClick(object sender, EditPriceEventHandler e)
        {

        }
        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            string list = "";
            Dictionary<int, int> id_amount = new Dictionary<int, int>();

            for (int i = 0; i < labelListControls.Count; i++)
            {
                int id = labelListControls[i].Model.Idnomenklatura;
                if (labelListControls[i].Selected)
                {
                    list += list == "" ? $"{id}" : $", {id}";
                    id_amount.Add(id, labelListControls[i].Amount);
                }
            }

            string sql =
                $"SELECT nomenklatura.*, price.price FROM kassa.nomenklatura " +
                $"inner join price on nomenklatura.idnomenklatura = price.idnomenclatura " +
                $"where idnomenclatura in ({list}) ";

            DataTable tmp = DBWrapper.Select(sql);

            DataTable json_tmp = new DataTable();
            json_tmp.Columns.Add("string_1");
            json_tmp.Columns.Add("string_2");
            json_tmp.Columns.Add("price");

            for(int i = 0; i < tmp.Rows.Count; i++)
            {
                int id = int.Parse(tmp.Rows[i]["idnomenklatura"].ToString());

                for (int j = 0; j < id_amount[id]; j++)
                {
                    DataRow dr = json_tmp.NewRow();
                    dr["string_1"] = tmp.Rows[i]["string_1"].ToString();
                    dr["string_2"] = tmp.Rows[i]["string_2"].ToString();

                    int pr = (int)(float.Parse(tmp.Rows[i]["price"].ToString()));

                    dr["price"] = pr.ToString();
                    json_tmp.Rows.Add(dr);
                }
            }

            string json = JsonConvert.SerializeObject(json_tmp);

            //File.WriteAllText($"{Option.ReportFolder}1234.json", json);

            UriReportSource uriReportSource = new UriReportSource();
            uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//NomLabel.trdp";
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
            report.ReportSource = uriReportSource;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string t = (sender as System.Windows.Controls.TextBox).Text;
            NomListBox.Items.Clear();
            for (int i = 0; i < labelListControls.Count; i++)
            {
                if (labelListControls[i].ContainText(t))
                {
                    NomListBox.Items.Add(labelListControls[i]);
                }
            }
        }
        #endregion
    }
}
