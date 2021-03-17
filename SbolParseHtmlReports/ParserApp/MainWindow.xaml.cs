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
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

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

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                var flowDoc = new FlowDocument();
                var table = new Table();                
                int numberOfColumns = 8;
                for (int x = 0; x < numberOfColumns; x++)
                {
                    table.Columns.Add(new TableColumn());

                    // Set alternating background colors for the middle colums.
                    if (x % 2 == 0)
                        table.Columns[x].Background = Brushes.Beige;
                    else
                        table.Columns[x].Background = Brushes.LightSteelBlue;                    
                }
                var titleGroup = new TableRowGroup();
                var titleRow = new TableRow();
                titleGroup.Rows.Add(titleRow);

                titleRow.Background = Brushes.Silver;
                titleRow.FontSize = 40;
                titleRow.FontWeight = FontWeights.Bold;
                titleRow.Cells.Add(new TableCell(new Paragraph(new Run("Данные отчета СБОЛ"))));
                titleRow.Cells[0].ColumnSpan = 6;

                table.RowGroups.Add(titleGroup);
                var tgroup = new TableRowGroup();
                foreach(var item in _parser.Operations)
                {
                    var tRow = new TableRow
                    {
                        FontSize = 12,
                        FontWeight = FontWeights.Normal
                    };
                    tRow.Cells.Add(new TableCell(new Paragraph(new Run(item.RowNumber.ToString()))));
                    tRow.Cells.Add(new TableCell(new Paragraph(new Run(item.Date.ToString()))));
                    tRow.Cells.Add(new TableCell(new Paragraph(new Run(item.Category))));
                    tRow.Cells.Add(new TableCell(new Paragraph(new Run(item.Title))));
                    tRow.Cells.Add(new TableCell(new Paragraph(new Run(item.Summ.ToString()))));
                    tRow.Cells.Add(new TableCell(new Paragraph(new Run(item.BalanceAfter.ToString()))));
                    tRow.Cells.Add(new TableCell(new Paragraph(new Run(item.Location))));
                    tRow.Cells.Add(new TableCell(new Paragraph(new Run(item.ProcessDate.ToString()))));
                    
                    tgroup.Rows.Add(tRow);
                }
                table.RowGroups.Add(tgroup);
                flowDoc.Blocks.Add(table);
                printDialog.PrintDocument(((IDocumentPaginatorSource)flowDoc).DocumentPaginator, "Csv");
            }
        }
    }
}
