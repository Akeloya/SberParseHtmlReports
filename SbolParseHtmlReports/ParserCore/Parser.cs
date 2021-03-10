using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ParserCore
{
    public class Parser
    {
        private string _path;
        private string _outPath;
        private DataSet _dataSet;
        private const string _rootTable = "//div[contains(@class,'b-trs')]";
        private const string _restOnBegin = "//*[contains(@class,'state_list')]/li[1]/div[2]";
        private List<CardOperation> _operations = new List<CardOperation>();
        private char _delimetr;
        public Parser(string path, string outPath, DataSet ds, char delimetr)
        {
            _path = path;
            _outPath = outPath;
            _dataSet = ds;
            _delimetr = delimetr;
        }

        public void RunParse()
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(_path);

            var root = htmlDoc.DocumentNode.SelectSingleNode(_rootTable);//"//*[@id=\"History\"]/div[5]/div[3]"
            var sumStr = htmlDoc.DocumentNode.SelectSingleNode(_restOnBegin).InnerText;
            var rest = decimal.Parse(sumStr.Trim().Replace('\u202F', ' '));
            if (root != null)
            {
                int rowNum = 1;
                var nodes = root.SelectNodes(_dataSet.DataXPath);
                foreach (var node in nodes)
                {
                    rest = GetValues(rest, rowNum, node);
                }
            }            

            if(_operations.Count > 0)
            {
                if (File.Exists(_outPath))
                    File.Delete(_outPath);
                var file = File.AppendText(_outPath);
                StringBuilder headerSb = BuildHeader();
                file.WriteLine(headerSb);

                foreach (var operation in _operations)
                {
                    StringBuilder sb = BuildRow(operation);
                    file.WriteLine(sb);
                }
                file.Close();
            }
        }

        private StringBuilder BuildRow(CardOperation operation)
        {
            var sb = new StringBuilder();
            sb.Append(operation.RowNumber)
                .Append(_delimetr)
                .Append(operation.Date)
                .Append(_delimetr)
                .Append(operation.ProcessDate)
                .Append(_delimetr)
                .Append(operation.Category)
                .Append(_delimetr)
                .Append(operation.Summ)
                .Append(_delimetr)
                .Append(operation.BalanceAfter)
                .Append(_delimetr)
                .Append(operation.Title)
                .Append(_delimetr)
                .Append(operation.Location)
                .Append(_delimetr);
            return sb;
        }

        private StringBuilder BuildHeader()
        {
            var headerSb = new StringBuilder();
            headerSb.Append("N#")
                .Append(_delimetr)
                .Append(_dataSet.Date.Name)
                .Append(_delimetr)
                .Append(_dataSet.DateProceed.Name)
                .Append(_delimetr)
                .Append(_dataSet.Category.Name)
                .Append(_delimetr)
                .Append(_dataSet.Summ.Name)
                .Append(_delimetr)
                .Append(_dataSet.BalanceArter.Name)
                .Append(_delimetr)
                .Append(_dataSet.Title.Name)
                .Append(_delimetr)
                .Append(_dataSet.Location.Name)
                .Append(_delimetr);
            return headerSb;
        }

        private decimal GetValues(decimal rest, int rowNum, HtmlNode node)
        {
            var title = node.SelectSingleNode(_dataSet.Title.XPath).InnerText.Trim();
            var sumNode = node.SelectSingleNode(_dataSet.Summ.XPath);
            var factor = -1;
            if (sumNode.FirstChild.Attributes["class"].Value == "trs_st-refill")
                factor = 1;

            var currSumStr = sumNode.SelectSingleNode(".//*[contains(@class, 'trs_sum-am')]").InnerText;
            currSumStr = currSumStr.Trim().Replace('\u202F', ' ');
            var sum = decimal.Parse(currSumStr);
            var categoty = node.SelectSingleNode(_dataSet.Category.XPath).InnerText.Trim();
            var location = node.SelectSingleNode(_dataSet.Location.XPath).InnerText.Trim();
            var date = node.SelectSingleNode(_dataSet.Date.XPath).InnerText;
            var opDate = node.SelectSingleNode(_dataSet.DateProceed.XPath).InnerText;

            //Платежи идут в обратном порядке, поэтому остаток формируется неверным
            rest = rest + factor * sum;
            _operations.Add(new CardOperation
            {
                RowNumber = rowNum,
                Title = title,
                Category = categoty,
                Date = DateTime.Parse(date),
                Location = location,
                ProcessDate = DateTime.Parse(opDate),
                Summ = sum * factor,
                BalanceAfter = rest
            });
            return rest;
        }
    }
}
