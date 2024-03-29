﻿using HtmlAgilityPack;

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ParserCore
{
    public class Parser
    {
        private readonly string _path;
        private readonly IDataSet _dataSet;
        private readonly List<CardOperation> _operations = new List<CardOperation>();
        private readonly char _delimetr;

        public IEnumerable<CardOperation> Operations => _operations;
        public Parser(string path, string settingsPath, char delimetr)
        {
            _path = path;
            _dataSet = DataSet.LoadSettings(settingsPath);
            _delimetr = delimetr;
        }

        public Parser(string path, IDataSet ds, char delimetr)
        {
            _path = path;
            _dataSet = ds;
            _delimetr = delimetr;
        }

        public void RunParse()
        {
            var htmlDoc = new HtmlDocument();
            var encoding = Encoding.GetEncoding(_dataSet.EncodingPage);
            htmlDoc.Load(_path, encoding);

            var root = htmlDoc.DocumentNode.SelectSingleNode(_dataSet.RootTableXpath);
            var sumStr = htmlDoc.DocumentNode.SelectSingleNode(_dataSet.RestXPath).InnerText?.Trim();
            var rest = sumStr.AsDecimal();

            if (root != null)
            {
                int rowNum = 1;
                var nodes = root.SelectNodes(_dataSet.DataXPath);
                foreach (var node in nodes)
                {
                    rest = GetValues(rest, rowNum++, node);
                }
            }
        }
        /// <summary>
        /// Saves data to file
        /// </summary>
        /// <param name="filePath">full path with file name and extension</param>
        public void Save(string filePath)
        {
            if (_operations.Count <= 0)
                return;
            if (File.Exists(filePath))
                File.Delete(filePath);
            using var file = File.AppendText(filePath);
            StringBuilder headerSb = BuildHeader();
            file.WriteLine(headerSb);

            foreach (var operation in _operations)
            {
                StringBuilder sb = BuildRow(operation);
                file.WriteLine(sb);
            }
            file.Close();
        }

        public async Task SaveAsync(string filePath)
        {
            if (_operations.Count <= 0)
                return;
            if (File.Exists(filePath))
                File.Delete(filePath);

            using var file = File.AppendText(filePath);
            StringBuilder headerSb = BuildHeader();
            await file.WriteLineAsync(headerSb.ToString());

            foreach (var operation in _operations)
            {
                StringBuilder sb = BuildRow(operation);
                await file.WriteLineAsync(sb.ToString());
            }
            file.Close();
        }

        private StringBuilder BuildRow(CardOperation operation)
        {
            var sb = new StringBuilder();
            sb.Append(operation.RowNumber).Append(_delimetr)
                .Append(operation.Date).Append(_delimetr)
                .Append(operation.ProcessDate).Append(_delimetr)
                .Append(operation.Category).Append(_delimetr)
                .Append(operation.Summ).Append(_delimetr)
                .Append(operation.BalanceAfter).Append(_delimetr)
                .Append(operation.Title).Append(_delimetr)
                .Append(operation.Location).Append(_delimetr);
            return sb;
        }

        private StringBuilder BuildHeader()
        {
            var headerSb = new StringBuilder();
            headerSb.Append("N#").Append(_delimetr)
                .Append(_dataSet.Date.Name).Append(_delimetr)
                .Append(_dataSet.DateProceed.Name).Append(_delimetr)
                .Append(_dataSet.Category.Name).Append(_delimetr)
                .Append(_dataSet.Summ.Name).Append(_delimetr)
                .Append(_dataSet.BalanceArter.Name).Append(_delimetr)
                .Append(_dataSet.Title.Name).Append(_delimetr)
                .Append(_dataSet.Location.Name).Append(_delimetr);
            return headerSb;
        }

        private decimal GetValues(decimal rest, int rowNum, HtmlNode node)
        {
            var sumNode = node.SelectSingleNode(_dataSet.Summ.XPath);
            var factor = -1;
            if (sumNode.FirstChild.Attributes["class"].Value == "trs_st-refill")
                factor = 1;

            var sum = GetNodeValue(node, _dataSet.Summ).AsDecimal() * factor;
            rest += sum;
            _operations.Add(new CardOperation
            {
                RowNumber = rowNum,
                Title = GetNodeValue(node, _dataSet.Title),
                Category = GetNodeValue(node, _dataSet.Category),
                Date = GetNodeValue(node, _dataSet.Date).AsDate(),
                Location = GetNodeValue(node, _dataSet.Location),
                ProcessDate = GetNodeValue(node, _dataSet.DateProceed).AsDate(),
                Summ = sum,
                BalanceAfter = rest
            });
            return rest;
        }

        private static string GetNodeValue(HtmlNode node, DataColumn col)
        {
            string result = null;
            var selectedNode = node.SelectSingleNode(col.XPath);
            if (selectedNode != null)
            {
                if (col.ContainerType == DataContainerType.InnerText)
                    result = selectedNode.InnerText;
                else
                    result = selectedNode.Attributes[col.AttributeName].Value;
            }

            if (!string.IsNullOrEmpty(result))
                result = result.Trim();
            return result;
        }
    }
}
