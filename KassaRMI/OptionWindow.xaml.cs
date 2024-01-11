using System.Windows;

namespace KassaRMI
{
    /// <summary>
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow
    {
        public OptionWindow()
        {
            InitializeComponent();
            DialogResult = true;
        }

        private void CloseBt_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
