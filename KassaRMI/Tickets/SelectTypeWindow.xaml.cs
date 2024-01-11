using System;
using System.Collections.Generic;
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

namespace KassaRMI.Tickets
{
    /// <summary>
    /// Interaction logic for SelectTypeWindow.xaml
    /// </summary>
    public partial class SelectTypeWindow
    {
        public bool notFree = false;

        public SelectTypeWindow()
        {
            InitializeComponent();
            DialogResult = true;
        }

        private void freeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void notfreeButton_Click(object sender, RoutedEventArgs e)
        {
            notFree = true;
            Close();
        }
    }
}
