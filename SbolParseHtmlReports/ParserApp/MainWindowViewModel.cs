using Caliburn.Micro;

using Microsoft.WindowsAPICodePack.Dialogs;

using ParserApp.Controls;
using ParserApp.Services;

using ParserCore;

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using WpfExtendedControls;

using Parser = ParserCore.Parser;

namespace ParserApp
{
    public class MainWindowViewModel : Screen
    {
        private Parser _parser;
        private readonly IDialogService _dialogService;
        public MainWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            DisplayName = Properties.Resources.MainWindowTitle;
        }
        public BindableCollection<CardOperation> Operations { get; set; }

        public Task CloseAsync()
        {
            return TryCloseAsync();
        }

        public Task AboutAsync()
        {
            var licenses = new List<LicenseInformation>
            {
                new LicenseInformation("Application", Encoding.UTF8.GetString(Properties.Resources.LICENSE), false)
            };
            var ab = new AboutApp(licenses, null);
            ab.Show();
            return Task.CompletedTask;
        }

        public Task OpenSettingsAsync()
        {
            return _dialogService.ShowAsync(new EditSettingsViewModel());
        }

        public Task OpenHtmlFileAsync()
        {
            return _dialogService.OpenFileDialog("Выберите отчёт", "Отчеты СБОЛ", "*.html;*.pdf", (fileName) =>
            {
                _parser = Parser.Get(fileName, App.GetSettingsPath(), ';');
                _parser.RunParse();
                Operations = new BindableCollection<CardOperation>(_parser.Operations);
                NotifyOfPropertyChange(nameof(Operations));
                return Task.CompletedTask;
            });
        }

        public Task OpenCsvFileAsync()
        {
            return _dialogService.ShowPupupAsync("Не реализованно!");
        }
        public bool CanSave_CanExecute => _parser != null && _parser.Operations.Any();
        public Task SaveAsync()
        {
            return _dialogService.OpenFileDialog(null, "(Html отчёт)", "*.html", (fileName) =>
            {
                var filePath = Path.Combine(fileName, "result.csv");
                return _parser.SaveAsync(filePath);
            });
        }

        public Task PrintAsync()
        {
            _dialogService.PrintDialog((printDialog) =>
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
                foreach (var item in _parser.Operations)
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
                return Task.CompletedTask;
            });

            return Task.CompletedTask;
        }

        public Task HelpAsync()
        {
            return Task.Run(() => Process.Start("https://github.com/Akeloya/SberParseHtmlReports/issues"));
        }
    }
}
