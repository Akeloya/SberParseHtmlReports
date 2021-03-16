using Microsoft.Win32;
using ParserApp.Controls;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfExtendedControls;
using ParserCore;

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
            var licenses = new List<LicenseInformation>
            {
                new LicenseInformation("Application", Encoding.UTF8.GetString(Properties.Resources.LICENSE), false)
            };
            var ab = new AboutApp(licenses,null);
            ab.Show();
        }

        private void OpenSettings_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppWindows.OpenSettings();
        }

        private void OpenHtmlFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "(Html отчёт)|*.html"
            };

            var result = ofd.ShowDialog();
            if(result == true)
            {
                var fileName = ofd.FileName;
                var ds = DataSet.LoadSettings(App.GetSettingsPath());
                var parser = new Parser(fileName, ds, ';');
                parser.RunParse();
                DgResult.ItemsSource = parser.Operations;
            }
        }

        private void OpenCsvFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}
