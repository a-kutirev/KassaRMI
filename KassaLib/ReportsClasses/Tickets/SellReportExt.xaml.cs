using KassaLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using Telerik.Reporting;
using Telerik.Windows.Controls;

namespace KassaLib.ReportsClasses.Tickets
{
    /// <summary>
    /// Interaction logic for SellReportExt.xaml
    /// </summary>
    public partial class SellReportExt
    {
        #region Members
        #endregion

        #region Ctor
        public SellReportExt()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            ReportDateFrom.SelectedValue = Option.CurrentDate;
            ReportDateTo.SelectedValue = Option.CurrentDate;
        }
        #endregion

        #region Events
        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {

            #region Таблицы продаж и поступлений
            DateTime start = (DateTime)ReportDateFrom.SelectedValue;
            DateTime end = (DateTime)ReportDateTo.SelectedValue;

            string sqlprihod = $"select * from newtickets where data <= '{end.ToString("yyyy-MM-dd")}' order by data asc";
            string sqlsell = $"select * from free_ticket_sell where date_ <= '{end.ToString("yyyy-MM-dd")}' order by date_ asc";

            List<NewTicketsModel> prihod = (List<NewTicketsModel>)DBWrapper.Select(sqlprihod).ToList<NewTicketsModel>(); ;
            List<free_ticket_sellModel> sell = (List<free_ticket_sellModel>)DBWrapper.Select(sqlsell).ToList<free_ticket_sellModel>();
            #endregion

            #region Вычисление стартового номера билета
            int Num_tail = 0;
            string Lit_tail = "";
            DateTime? StartNumCount = null;

            for (int i = 0; i < prihod.Count; i++)
                if (start >= prihod[i].Data && prihod[i].Usenewnumeration == 1)
                {
                    Num_tail = prihod[i].Num_tail;
                    Lit_tail = prihod[i].Liter_tail;
                    StartNumCount = prihod[i].Data;
                };
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
            for (int i = 0; i < sell.Count; i++)
            {
                if (StartNumCount > sell[i].Date_) continue;

                string txt = $"{Lit_tail}{(Num_tail.ToString())}-{Lit_tail}{(Num_tail + sell[i].Amount - 1).ToString()}";

                Num_tail += sell[i].Amount;
                if (detail.ContainsKey(sell[i].Date_))
                    detail[sell[i].Date_].TicketsNum = txt;
            }
            #endregion

            #region Конвертация в таблицу и json

            DataTable tmp = new DataTable();

            tmp.Columns.Add("ttt");
            tmp.Columns.Add("tickets_num");
            tmp.Columns.Add("date");
            tmp.Columns.Add("balance_start");
            tmp.Columns.Add("prihod");
            tmp.Columns.Add("sell");
            tmp.Columns.Add("balance_end");

            for(int i = 0; i < detail.Count; i++)
            {
                DataRow dr = tmp.NewRow();
                if (detail.ElementAt(i).Value == null) continue;
                ExtendedSellTem esi = detail.ElementAt(i).Value;

                dr["ttt"] = "111";
                dr["tickets_num"] = esi.TicketsNum;
                dr["date"] = esi.Date.ToString("dd.MM.yyyy");
                dr["balance_start"] = esi.BalanceStart;
                dr["prihod"] = esi.Prihod == 0 ? "-" : esi.Prihod.ToString();
                dr["sell"] = esi.Sell == 0 ? "-" : esi.Sell.ToString();
                dr["balance_end"] = esi.BalanceEnd;

                tmp.Rows.Add(dr);
            }

            string json = JsonConvert.SerializeObject(tmp);
            //File.WriteAllText($"{Option.ReportFolder}11.json", json);

            string header = $"Отчет о выданных билетах за период с " +
                    $"{start.ToString("dd.MM.yyyy")} по " +
                    $"{end.ToString("dd.MM.yyyy")}";

            UriReportSource uriReportSource = new UriReportSource();
            uriReportSource.Uri = AppDomain.CurrentDomain.BaseDirectory + "Reports//SellTicketExt.trdp";
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("json_source", json));
            uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("header", header));
            report.ReportSource = uriReportSource;

            #endregion
        }
        #endregion
    }

    public class ExtendedSellTem
    {
        public string TicketsNum { get; set; }
        public DateTime Date { get; set; }
        public int BalanceStart { get; set; } = 0;
        public int BalanceEnd { get; set; } = 0;
        public int Sell { get; set; } = 0;
        public int Prihod { get; set; }  

        public ExtendedSellTem(DateTime d)
        {
            Date = d;
        }
    }
}
