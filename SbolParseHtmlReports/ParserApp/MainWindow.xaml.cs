using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfExtendedControls;

namespace ParserApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void About_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var licenses = new List<LicenseInformation>();
            licenses.Add(new LicenseInformation("Application", Encoding.UTF8.GetString(Properties.Resources.LICENSE), false));
            var ab = new AboutApp(licenses,null);
            ab.Show();
        }
    }
}
