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
        private readonly string _path;
        private DataSet _dataSet;
        private List<CardOperation> _operations = new List<CardOperation>();
        private char _delimetr;
        public IEnumerable<CardOperation> Operations => _operations;
        public Parser(string path, DataSet ds, char delimetr)
        {
            _path = path;
            _dataSet = ds;
            _delimetr = delimetr;
        }

        public void RunParse()
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(_path);

            var root = htmlDoc.DocumentNode.SelectSingleNode(_dataSet.RootTableXpath);
            var sumStr = htmlDoc.DocumentNode.SelectSingleNode(_dataSet.RestXPath).InnerText;
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
        }

        public void Save(string filePath)
        {
            if (_operations.Count > 0)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                var file = File.AppendText(filePath);
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
            var sumNode = node.SelectSingleNode(_dataSet.Summ.XPath);
            var factor = -1;
            if (sumNode.FirstChild.Attributes["class"].Value == "trs_st-refill")
                factor = 1;

            var currSumStr = GetNodeValue(node, ".//*[contains(@class, 'trs_sum-am')]").Replace('\u202F', ' ');
            var sum = decimal.Parse(currSumStr);
            var date = GetNodeValue(node, _dataSet.Date.XPath);
            var opDate = GetNodeValue(node, _dataSet.DateProceed.XPath);

            rest += factor * sum;
            _operations.Add(new CardOperation
            {
                RowNumber = rowNum,
                Title = GetNodeValue(node, _dataSet.Title.XPath),
                Category = GetNodeValue(node, _dataSet.Category.XPath),
                Date = DateTime.Parse(date),
                Location = GetNodeValue(node, _dataSet.Location.XPath),
                ProcessDate = DateTime.Parse(opDate),
                Summ = sum * factor,
                BalanceAfter = rest
            });
            return rest;
        }

        private static string GetNodeValue(HtmlNode node, string path)
        {
            string result = null;
            var selectedNode = node.SelectSingleNode(path);
            if (selectedNode != null)
                result = selectedNode.InnerText;
            if (!string.IsNullOrEmpty(result))
                result = result.Trim();
            return result;
        }
    }
}
