using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Layout;

using System;
using System.IO;

namespace ParserCore
{
    internal class PdfParser : Parser
    {
        internal PdfParser(string path, string settingsPath, char delimeter) : base(path, settingsPath, delimeter)
        {

        }

        internal PdfParser(string path, IDataSet ds, char delimeter) : base(path, ds, delimeter)
        {

        }

        public override void RunParse()
        {
            _operations.Clear();
            using var pdfReader = new PdfReader(_path);
            using PdfDocument d = new PdfDocument(pdfReader);
            using var doc = new Document(d);

            Rectangle rect = new Rectangle(0, 0, 1000, 1000);

            var fontFilter = new TextRegionEventFilter(rect);

            var tempFile = System.IO.Path.GetTempFileName();

            var pages = d.GetPageLabels();
            for (var i = 1; i <= d.GetNumberOfPages(); i++)
            {
                FilteredEventListener listener = new FilteredEventListener();
                LocationTextExtractionStrategy extractionStrategy = listener
                    .AttachEventListener(new LocationTextExtractionStrategy(), fontFilter);
                PdfCanvasProcessor parser = new(listener);
                var page = d.GetPage(i);
                parser.ProcessPageContent(page);
                string actualText = extractionStrategy.GetResultantText();
                File.AppendAllText(tempFile, actualText);                
            }
            doc.Close();

            var allLines = File.ReadAllLines(tempFile);

            for (var lineNumber = 0; lineNumber < allLines.Length; lineNumber++)
            {
                var workingLine = allLines[lineNumber].Replace(((char)160).ToString(), "");

                var parts = workingLine.Split([" "], StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 5)
                    continue;

                if (!double.TryParse(parts[parts.Length - 2], out var amount))
                    continue;

                var debt = 0d;
                var credit = 0d;
                if (!parts[parts.Length - 2].StartsWith("+"))
                {
                    credit = amount;
                    amount *= -1;
                }
                else
                    debt = amount;


                if (!double.TryParse(parts[parts.Length - 1], out var rest))
                    continue;

                if (!(DateTime.TryParse(parts[0], out var date) && TimeSpan.TryParse(parts[1], out var time)))
                {
                    continue;
                }

                var operationDate = date + time;
                var destination = parts[3];
                var destCard = string.Empty;
                var sourceCard = string.Empty;
                for (var i = 4; i < parts.Length - 2; i++)
                {
                    destination += " " + parts[i];
                }

                if (destination.Contains("Перевод", StringComparison.InvariantCultureIgnoreCase))
                {
                    var notParsedDest = allLines[lineNumber + 1].Replace(((char)160).ToString(), "");
                    var notParsedSplit = notParsedDest.Split([" "], StringSplitOptions.RemoveEmptyEntries);
                    for (var i = 0; i < notParsedSplit.Length; i++)
                    {
                        if (notParsedSplit[i].Contains("карту", StringComparison.InvariantCultureIgnoreCase))
                            destCard = notParsedSplit[i + 1];
                        if (notParsedSplit[i].Contains("карты", StringComparison.InvariantCultureIgnoreCase))
                            sourceCard = notParsedSplit[i + 1];
                    }
                }
                var operation = new CardOperation()
                {
                    Date =operationDate,
                    Summ = (decimal)amount,
                    RowNumber = lineNumber + 1,
                    Category = destination,
                    
                };
                _operations.Add(operation);
                ///"ДАТА\tОперация\tСумма\tОстаток\tКарта\tСчёт\tПриход\tРасход"
                //$"{operationDate}\t{destination}\t{amount}\t{rest}\t{destCard ?? sourceCard}\t{card}\t{debt}\t{credit}";
            }
        }
    }
}
