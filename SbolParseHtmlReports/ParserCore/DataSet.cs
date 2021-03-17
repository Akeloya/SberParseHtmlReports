﻿using Newtonsoft.Json;
using ParserCore.Properties;
using System.IO;
using System.Text;

namespace ParserCore
{
    public class DataSet
    {
        public string EncodingValue { get; set; }
        public string DataXPath { get; set; }
        public string RestXPath { get; set; }
        public string RootTableXpath { get; set; }
        public DataColumn Category { get; set; }
        public DataColumn Date { get; set; }
        public DataColumn DateProceed { get; set; }
        public DataColumn Location { get; set; }
        public DataColumn Summ { get; set; }
        public DataColumn Title { get; set; }
        public DataColumn BalanceArter { get; set; }

        public DataSet()
        {
            EncodingValue = Encoding.UTF8.EncodingName;
            DataXPath = "//div[contains(@class,'trs_it')]";
            RootTableXpath = "//div[contains(@class,'b-trs')]";
            RestXPath = "//*[contains(@class,'state_list')]/li[1]/div[2]";
            Title = new DataColumn("Название операции", ".//div/div[1]", DataContainerType.InnerText);
            Category = new DataColumn("Категория", ".//*[contains(@class,'icat')]", DataContainerType.InnerText);
            Date = new DataColumn("Дата операции", ".//div[contains(@class,'trs_date')]/span", DataContainerType.Attribute, "data-date");
            Summ = new DataColumn("Сумма", ".//div[contains(@class,'trs_sum')]", DataContainerType.InnerText);
            BalanceArter = new DataColumn("Остаток на карте",null, DataContainerType.InnerText);
            Location = new DataColumn("Место", "//span[contains(@class,'trs_country')]", DataContainerType.InnerText);
            DateProceed = new DataColumn("Дата проведения", "//span[contains(@class,'idate')]", DataContainerType.Attribute, "data-date");
        }

        public static DataSet LoadSettings(string jsonPath)
        {
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException(Resource.SettingsFileNotFoundExceptionMessage, jsonPath);
            var json = File.ReadAllText(jsonPath);
            var obj = JsonConvert.DeserializeObject<DataSet>(json);
            return obj;
        }

        public void Save(string jsonPath)
        {
            var json = JsonConvert.SerializeObject(this);
            if (File.Exists(jsonPath))
                File.Delete(jsonPath);
            File.WriteAllText(jsonPath, json);
        }
    }
}
