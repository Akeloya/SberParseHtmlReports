using ParserApp.Controls;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfExtendedControls;
using ParserCore;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Linq;
using System.IO;

namespace ParserApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Parser _parser;

        public IEnumerable<CardOperation> Operations
        {
            get { return (IEnumerable<CardOperation>) GetValue(OperationsProperty); }
            set { SetValue(OperationsProperty, value); }
        }

        public static readonly DependencyProperty OperationsProperty = DependencyProperty.Register(nameof(Operations),
            typeof(IEnumerable<CardOperation>),
            typeof(MainWindow));
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
            var ofd = new CommonOpenFileDialog();
            ofd.Filters.Add(new CommonFileDialogFilter("(Html отчёт)", "*.html"));

            var result = ofd.ShowDialog();
            if(result == CommonFileDialogResult.Ok)
            {
                var fileName = ofd.FileName;
                var ds = DataSet.LoadSettings(App.GetSettingsPath());
                _parser = new Parser(fileName, ds, ';');
                _parser.RunParse();
                Operations = _parser.Operations;
            }
        }

        private void OpenCsvFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var ofd = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
            };

            var result = ofd.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                var fileName = Path.Combine(ofd.FileName, "result.csv");
                _parser.Save(fileName);
            }
            
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _parser != null && _parser.Operations.Any();
        }

    }
}
