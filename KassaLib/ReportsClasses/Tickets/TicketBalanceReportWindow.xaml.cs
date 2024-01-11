using KassaLib.Models;
using Newtonsoft.Json;
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
using Telerik.Reporting;
using Telerik.Windows.Controls;

namespace KassaLib.ReportsClasses.Tickets
{
    /// <summary>
    /// Interaction logic for TicketBalanceReportWindow.xaml
    /// </summary>
    public partial class TicketBalanceReportWindow
    {
        #region Members
        #endregion

        #region  Ctor
        public TicketBalanceReportWindow()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();
            DialogResult = true;

            ReportDate.SelectedValue = Option.CurrentDate;
        }
        #endregion

        #region Events
        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            #region Таблицы продаж и поступлений
            DateTime start = new DateTime(1,1,1);
            DateTime end = (DateTime)ReportDate.SelectedValue;

            string sqlprihod = $"select * from newtickets where data <= '{end.ToString("yyyy-MM-dd")}' order by data asc";
            string sqlsell = $"select * from free_ticket_sell where date_ <= '{end.ToString("yyyy-MM-dd")}' order by date_ asc";

            List<NewTicketsModel> prihod = (List<NewTicketsModel>)DBWrapper.Select(sqlprihod).ToList<NewTicketsModel>(); ;
            List<free_ticket_sellModel> sell = (List<free_ticket_sellModel>)DBWrapper.Select(sqlsell).ToList<free_ticket_sellModel>();
            #endregion

            #region Вычисление стартового номера билета
            int Num_tail = 0;
            string Lit_tail = "";
            DateTime? StartNumCount = null;

            DateTime tmp = start;
            NewTicketsModel MaxDatePrihod = prihod[0];

            for (int i = 0; i < prihod.Count; i++)
            {
                if (prihod[i].Data > MaxDatePrihod.Data)
                    MaxDatePrihod = prihod[i];

                if (tmp <= prihod[i].Data && prihod[i].Usenewnumeration == 1)
                {
                    Num_tail = prihod[i].Num_tail;
                    Lit_tail = prihod[i].Liter_tail;
                    StartNumCount = prihod[i].Data;
                    tmp = (DateTime)StartNumCount;
                };
            }

            start = tmp;
            #endregion

            #region Расчет количества на начало периода

            int balanceOnStartDate = 0;
            for (int i = 0; i < prihod.Count; i++)
            {
                if (prihod[i].Data >= start) continue;
                balanceOnStartDate += prihod[i].Amount;
            }
            for (int i = 0; i < sell.Count; i++)
            {
                if (sell[i].Date_ >= start) continue;
                balanceOnStartDate -= sell[i].Amount;
            }
            #endregion

            #region Подотовка структуры

            SortedDictionary<DateTime, ExtendedSellTem> detail =
                new SortedDictionary<DateTime, ExtendedSellTem>();

            SortedDictionary<DateTime, NewTicketsModel> detailPrihod =
                new SortedDictionary<DateTime, NewTicketsModel>();

            SortedDictionary<DateTime, free_ticket_sellModel> detailSell =
                new SortedDictionary<DateTime, free_ticket_sellModel>();

            for (int i = 0; i < prihod.Count; i++)
                detailPrihod.Add(prihod[i].Data, prihod[i]);

            for (int i = 0; i < sell.Count; i++)
                detailSell.Add(sell[i].Date_, sell[i]);

            // Начальная структура
            for (DateTime dt = start; dt <= end; dt = dt.AddDays(1))
                detail.Add(dt, null);

            // Заполнение остатков, прихода, расхода
            int balance = balanceOnStartDate;
            for (int i = 0; i < detail.Count; i++)
            {
                DateTime key = detail.ElementAt(i).Key;

                if (detailPrihod.ContainsKey(key) || detailSell.ContainsKey(key))
                {
                    detail[key] = new ExtendedSellTem(key);

                    if (detailPrihod.ContainsKey(key))
                        detail[key].Prihod = detailPrihod[key].Amount;
                    if (detailSell.ContainsKey(key))
                        detail[key].Sell = detailSell[key].Amount;

                    detail[key].BalanceStart = balance;
                    detail[key].BalanceEnd = detail[key].BalanceStart;

                    if (detailPrihod.ContainsKey(key))
                        detail[key].BalanceEnd += detailPrihod[key].Amount;
                    if (detailSell.ContainsKey(key))
                        detail[key].BalanceEnd -= detailSell[key].Amount;

                    balance = detail[key].BalanceEnd;
                }
            }
            #endregion

            #region Номера билетов
            int MaxNumberTicket = MaxDatePrihod.Num_tail + MaxDatePrihod.Amount - 1;

            for (int i = 0; i < sell.Count; i++)
            {
                if (StartNumCount > sell[i].Date_) continue;

                Num_tail += sell[i].Amount;
            }
            #endregion

            DataTable tmpTbl = new DataTable();
            tmpTbl.Columns.Add("col1");
            tmpTbl.Columns.Add("col2");

            DataRow dr = tmpTbl.NewRow();
            dr["col1"] = "Количество оставшихся билетов:";
            dr["col2"] = balance;
            tmpTbl.Rows.Add(dr);

            dr = tmpTbl.NewRow();
            dr["col1"] = "Диапазон номеров билетов в остатке:";
            dr["col2"] = $"{MaxDatePrihod.Liter_tail}{Num_tail.ToString()} - {MaxDatePrihod.Liter_tail}{MaxNumberTicket.ToString()}";
            tmpTbl.Rows.Add(dr);

            string json = JsonConvert.SerializeObject(tmpTbl);
            string header = $"Остаток билетов на {end.ToString("dd MMMM yyyy")}г.";

            UriReportSource uriReportSource = new UriReportSource();
            uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//TicketBalanceReport.trdp";
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("header", header));
            report.ReportSource = uriReportSource;
        }
        #endregion
    }
}
