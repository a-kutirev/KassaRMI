using KassaLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace KassaLib
{
    public static class Option
    {
        public static DateTime CurrentDate;
        public static string ReportFolder;
        public static List<OptionsModel> optionsAll;

        #region Расчет количества временных экскурсий на дату
        public static int GetVremCount(DateTime d)
        {
            DateTime cur = new DateTime(1, 1, 1);
            int count_v = -1;
            for(int i = 0; i < optionsAll.Count; i++)
            {
                OptionsModel om = optionsAll[i];
                if((om.Option_date  > cur) && (om.Option_date <= d))
                {
                    cur = om.Option_date;
                    count_v  = int.Parse(optionsAll[i].Option_val);
                }
            }
            return count_v;
        }
        #endregion

        #region Расчет остатков на дату с ID
        public static Dictionary<int, int> CalculateIdBalanceOnDate(DateTime date, bool ShowZero)
        {
            string sql_prihod =
                                $"select idnomenclatura, sum(prihod_amount) as total from " +
                                $"(SELECT * FROM prihod where date_ < '{date.ToString("yyyy-MM-dd")}') t " +
                                $"group by idnomenclatura";

            string sql_sell =
                                $"select idnomenclatura, sum(amount) as total from " +
                                $"(SELECT * FROM sell where date_ < '{date.ToString("yyyy-MM-dd")}') t " +
                                $"group by idnomenclatura";

            DataTable prihod_table = DBWrapper.Select(sql_prihod);
            DataTable sell_table = DBWrapper.Select(sql_sell);

            Dictionary<int, int> prihodDict = new Dictionary<int, int>();
            Dictionary<int, int> sellDict = new Dictionary<int, int>();

            for (int i = 0; i < prihod_table.Rows.Count; i++)
            {
                int id = int.Parse(prihod_table.Rows[i]["idnomenclatura"].ToString());
                int total = int.Parse(prihod_table.Rows[i]["total"].ToString());

                prihodDict.Add(id, total);
            }
            for (int i = 0; i < sell_table.Rows.Count; i++)
            {
                int id = int.Parse(sell_table.Rows[i]["idnomenclatura"].ToString());
                int total = int.Parse(sell_table.Rows[i]["total"].ToString());

                sellDict.Add(id, total);
            }

            Dictionary<int, int> res = new Dictionary<int, int>();            

            for (int i = 0; i < prihodDict.Count; i++)
            {
                int id = prihodDict.ElementAt(i).Key;

                int bal = prihodDict[id] - (sellDict.ContainsKey(id) ? sellDict[id] : 0);
                if ((bal <= 0) && (!ShowZero)) continue;

                res.Add(id, bal);
            }

            return res;
        }
        #endregion 

        #region Расчет остатков на дату
        public static DataTable CalculateBalanceOnDateExt(DateTime date, bool ShowZero)
        {
            string sql_prihod =
                                $"select idnomenclatura, sum(prihod_amount) as total from " +
                                $"(SELECT * FROM prihod where date_ < '{date.ToString("yyyy-MM-dd")}') t " +
                                $"group by idnomenclatura";

            string sql_sell =
                                $"select idnomenclatura, sum(amount) as total from " +
                                $"(SELECT * FROM sell where date_ < '{date.ToString("yyyy-MM-dd")}') t " +
                                $"group by idnomenclatura";

            DataTable prihod_table = DBWrapper.Select(sql_prihod);
            DataTable sell_table = DBWrapper.Select(sql_sell);

            Dictionary<int, int> prihodDict = new Dictionary<int, int>();
            Dictionary<int, int> sellDict = new Dictionary<int, int>();

            for (int i = 0; i < prihod_table.Rows.Count; i++)
            {
                int id = int.Parse(prihod_table.Rows[i]["idnomenclatura"].ToString());
                int total = int.Parse(prihod_table.Rows[i]["total"].ToString());

                prihodDict.Add(id, total);
            }
            for (int i = 0; i < sell_table.Rows.Count; i++)
            {
                int id = int.Parse(sell_table.Rows[i]["idnomenclatura"].ToString());
                int total = int.Parse(sell_table.Rows[i]["total"].ToString());

                sellDict.Add(id, total);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("balance", typeof(int));

            for (int i = 0; i < prihodDict.Count; i++)
            {
                int id = prihodDict.ElementAt(i).Key;

                NomenklaturaModel nm = new NomenklaturaModel(id);

                int bal = prihodDict[id] - (sellDict.ContainsKey(id) ? sellDict[id] : 0);
                if ((bal <= 0) && (!ShowZero)) continue;

                DataRow dr = dt.NewRow();
                dr["id"] = id;
                dr["name"] = nm.Nomenklaturaname;
                dr["balance"] = bal;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static string CalculateBalanceOnDate(DateTime date, bool ShowZero)
        {
            string sql_prihod =
                                $"select idnomenclatura, sum(prihod_amount) as total from " +
                                $"(SELECT * FROM prihod where date_ < '{date.ToString("yyyy-MM-dd")}') t " +
                                $"group by idnomenclatura";

            string sql_sell =
                                $"select idnomenclatura, sum(amount) as total from " +
                                $"(SELECT * FROM sell where date_ < '{date.ToString("yyyy-MM-dd")}') t " +
                                $"group by idnomenclatura";

            DataTable prihod_table = DBWrapper.Select(sql_prihod);
            DataTable sell_table = DBWrapper.Select(sql_sell);

            Dictionary<int, int> prihodDict = new Dictionary<int, int>();
            Dictionary<int, int> sellDict = new Dictionary<int, int>();

            for (int i = 0; i < prihod_table.Rows.Count; i++)
            {
                int id = int.Parse(prihod_table.Rows[i]["idnomenclatura"].ToString());
                int total = int.Parse(prihod_table.Rows[i]["total"].ToString());

                prihodDict.Add(id, total);
            }
            for (int i = 0; i < sell_table.Rows.Count; i++)
            {
                int id = int.Parse(sell_table.Rows[i]["idnomenclatura"].ToString());
                int total = int.Parse(sell_table.Rows[i]["total"].ToString());

                sellDict.Add(id, total);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("balance", typeof(int));

            for (int i = 0; i < prihodDict.Count; i++)
            {
                int id = prihodDict.ElementAt(i).Key;

                NomenklaturaModel nm = new NomenklaturaModel(id);

                int bal = prihodDict[id] - (sellDict.ContainsKey(id) ? sellDict[id] : 0);
                if ((bal <= 0) && (!ShowZero)) continue;

                DataRow dr = dt.NewRow();
                dr["name"] = nm.Nomenklaturaname;              
                dr["balance"] = bal;
                dt.Rows.Add(dr);
            }

            return JsonConvert.SerializeObject(dt);
        }
        #endregion

        #region Расчет количества выданных билетов
        public static void RecalcTotal(DateTime dte)
        {
            string sql = $"select * from free_ticket_sell where date_ = '{dte.ToString("yyyy-MM-dd")}'";
            DataTable dt = DBWrapper.Select(sql);

            free_ticket_sellModel _free_Ticket_SellModel;

            if(dt.Rows.Count > 0)
            {
                int id = int.Parse(dt.Rows[0]["idfree_ticket_sell"].ToString());
                _free_Ticket_SellModel = new free_ticket_sellModel(id);
            }
            else
            {
                _free_Ticket_SellModel = new free_ticket_sellModel();
            }

            sql = $"SELECT * FROM kassa.FreeTicketStat where date(FreeTicketStatDate) = '{dte.ToString("yyyy-MM-dd")}'";
            dt = DBWrapper.Select(sql);

            int amount = 0;
            int amount_free = 0;
            int amount_notfree = 0;

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                int expoid = int.Parse(dt.Rows[i]["idexposition"].ToString());
                int am = int.Parse(dt.Rows[i]["amount"].ToString());

                amount += (expoid == 15) ? am * Option.GetVremCount(dte) : am;

                bool notfree = dt.Rows[i]["isnotfree"].ToString() == "1";

                if(notfree)
                    amount_notfree += (expoid == 15) ? am * Option.GetVremCount(dte) : am;
                else
                    amount_free += (expoid == 15) ? am * Option.GetVremCount(dte) : am;
            }

            _free_Ticket_SellModel.Amount = amount;
            _free_Ticket_SellModel.Amount_free = amount_free;
            _free_Ticket_SellModel.Amount_notfree = amount_notfree;

            if (_free_Ticket_SellModel.Idfree_ticket_sell == -1)
                _free_Ticket_SellModel.Insert();
            else
                _free_Ticket_SellModel.Update();
        }
        #endregion

        #region РАсчет остатков
        public static void CalculateBalance()
        {
            string sql_prihod = "select idnomenclatura, sum(prihod_amount) as total from prihod group by idnomenclatura";
            string sql_sell = "select idnomenclatura, sum(amount) as total from sell group by idnomenclatura";

            DataTable prihod_table = DBWrapper.Select(sql_prihod);
            DataTable sell_table = DBWrapper.Select(sql_sell);

            Dictionary<int, int> prihodDict = new Dictionary<int, int>();
            Dictionary<int, int> sellDict = new Dictionary<int, int>();

            for (int i = 0; i < prihod_table.Rows.Count; i++)
            {
                int id = int.Parse(prihod_table.Rows[i]["idnomenclatura"].ToString());
                int total = int.Parse(prihod_table.Rows[i]["total"].ToString());

                prihodDict.Add(id, total);
            }
            for (int i = 0; i < sell_table.Rows.Count; i++)
            {
                int id = int.Parse(sell_table.Rows[i]["idnomenclatura"].ToString());
                int total = int.Parse(sell_table.Rows[i]["total"].ToString());

                sellDict.Add(id, total);
            }
            for (int i = 0; i < prihodDict.Count; i++)
            {
                int id = prihodDict.ElementAt(i).Key;

                NomenklaturaModel nm = new NomenklaturaModel(id);

                nm.Balance = prihodDict.ElementAt(i).Value;
                if (sellDict.ContainsKey(id))
                    nm.Balance -= sellDict[id];

                nm.Update();
            }
        }
        #endregion
    }
}
