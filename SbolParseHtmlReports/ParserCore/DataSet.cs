using System;
using System.Collections.Generic;
using System.Text;

namespace ParserCore
{
    public class DataSet
    {
        public string DataXPath { get; }
        public DataColumn Category { get; }
        public DataColumn Date { get; }
        public DataColumn DateProceed { get; }
        public DataColumn Location { get; }
        public DataColumn Summ { get; }
        public DataColumn Title { get; }
        public DataColumn BalanceArter { get; }

        public DataSet()
        {
            DataXPath = "//div[contains(@class,'trs_it')]";
            Title = new DataColumn
            {
                Name = "Название операции",
                XPath = ".//div/div[1]",
            };
            Category = new DataColumn
            {
                Name = "Категория",
                XPath = ".//*[contains(@class,'icat')]",
            };
            Date = new DataColumn
            {
                Name = "Дата операции",
                XPath = ".//div[contains(@class,'trs_date')]",
            };
            Summ = new DataColumn
            {
                Name = "Сумма",
                XPath = ".//div[contains(@class,'trs_sum')]",
            };
            BalanceArter = new DataColumn
            {
                Name = "Остаток на карте",
            };
            Location = new DataColumn
            {
                Name = "Место",
                XPath = "//span[contains(@class,'trs_country')]",
            };
            DateProceed = new DataColumn
            {
                Name = "Дата проведения",
                XPath = "//span[contains(@class,'idate')]",
            };
        }       
    }
}
