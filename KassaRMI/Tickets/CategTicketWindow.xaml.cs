using KassaLib;
using KassaLib.Controls;
using KassaLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Telerik.Windows.Controls;

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for CategTicketWindow.xaml
    /// </summary>
    public partial class CategTicketWindow
    {
        #region Members
        #endregion

        #region Ctor
        public CategTicketWindow()
        {

            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();

            #region Экспозиции
            string sql = $"SELECT * FROM exposition where ('{Option.CurrentDate.ToString("yyyy-MM-dd")}' " +
                    $"between expositionstart and expositionend) or expositionpersistent = 1";
            List<ExpositionModel> vremList = new List<ExpositionModel>();
            List<ExpositionModel> expoList = (List<ExpositionModel>)DBWrapper.Select(sql).ToList<ExpositionModel>();

            List<int> zamExpos = new List<int>();
            for (int i = 0; i < expoList.Count; i++)
                if (expoList[i].Expositionzam) zamExpos.Add(expoList[i].Expositionzamexpo);

            for (int i = 0; i < expoList.Count; i++)
            {
                int id = expoList[i].Idexposition;
                if (zamExpos.Contains(id)) continue;

                if (expoList[i].Expositionvrem)
                {
                    vremList.Add(expoList[i]);
                    continue;
                }

                TextBlock tb = new TextBlock();
                tb.Text = expoList[i].Expositionname;
                tb.TextWrapping = TextWrapping.Wrap;

                CheckBox cb = new CheckBox();
                cb.Content = tb;
                cb.FontSize = 16;
                cb.FontWeight = FontWeights.Normal;
                cb.Tag = expoList[i].Idexposition;
                cb.Margin = new Thickness(5, 0, 5, 0);

                cb.Checked += Cb_Checked; ;
                cb.Unchecked += Cb_Checked;

                ExpoPanel.Children.Add(cb);
            }
            #endregion

            #region Категории            
            sql = "select * from category";
            List<CategoryModel> catList = (List<CategoryModel>)DBWrapper.Select(sql).ToList<CategoryModel>();

            for (int i = 0; i < catList.Count; i++)
            {
                CategoryControl cac = new CategoryControl();
                cac.Model = catList[i];
                CatPanel.Children.Add(cac);
            }
            #endregion

            SetDate();
        }

        private void SetDate()
        {
            DateLabel.Foreground = 
                (Option.CurrentDate.Date == DateTime.Now.Date) ?
                Brushes.Black : Brushes.Red;

            DateLabel.Content = $"{Option.CurrentDate.ToString("dd MMMM yy")} г.";
        }
        #endregion

        #region Events
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            SelectTypeWindow selectWin = new SelectTypeWindow();
            selectWin.ShowDialog();

            bool isFreeRes = selectWin.notFree;

            DateTime dt = Option.CurrentDate;
            int count_vrem = Option.GetVremCount(dt);
            string btName = (sender as Button).Name;

            for (int i = 0; i < ExpoPanel.Children.Count; i++)
            {
                CheckBox cb = ExpoPanel.Children[i] as CheckBox;
                int expoid = (int)cb.Tag;
                if ((bool)cb.IsChecked)
                {
                    for (int j = 0; j < CatPanel.Children.Count; j++)
                    {
                        CategoryControl cac = CatPanel.Children[j] as CategoryControl;
                        if (cac.IsActive && cac.Amount != 0)
                        {
                            FreeTicketStatModel m = new FreeTicketStatModel();

                            if(expoid == 40) m.Amount = cac.Amount * 4;
                            else             m.Amount = cac.Amount;
                            m.Isnotfree = isFreeRes;
                            m.Idexposition = expoid;
                            m.Idcategory = cac.Model.Idcategory;
                            m.FreeTicketStatDate = dt;
                            m.Insert();
                        }
                    }
                }
            }

            Option.RecalcTotal(Option.CurrentDate);
            MessageBox.Show("Данные сохранены");

            if (btName == "SaveAndCloseBt") Close();
            else
            {
                for (int i = 0; i < ExpoPanel.Children.Count; i++)
                {
                    CheckBox cb = ExpoPanel.Children[i] as CheckBox;
                    cb.IsChecked = false;
                }
                for (int j = 0; j < CatPanel.Children.Count; j++)
                {
                    CategoryControl cac = CatPanel.Children[j] as CategoryControl;
                    cac.IsActive = false;
                    cac.Amount = 0;
                }

            }
        }

        private void Cb_Checked(object sender, RoutedEventArgs e)
        {
            bool enableCat = false;
            for (int i = 0; i < ExpoPanel.Children.Count; i++)
            {
                if (!(ExpoPanel.Children[i] is CheckBox)) continue;

                CheckBox cb = (CheckBox)ExpoPanel.Children[i];
                if ((bool)cb.IsChecked)
                {
                    enableCat = true;
                    break;
                }
            }

            CatGroup.IsEnabled = enableCat;

            if (!enableCat)
                for (int i = 0; i < CatPanel.Children.Count; i++)
                {
                    CategoryControl cac = CatPanel.Children[i] as CategoryControl;
                    cac.IsActive = false;
                }
        }
        #endregion
    }
}
