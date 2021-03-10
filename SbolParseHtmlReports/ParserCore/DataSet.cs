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
            Title = new DataColumn("Название операции", ".//div/div[1]");
            Category = new DataColumn("Категория", ".//*[contains(@class,'icat')]");
            Date = new DataColumn("Дата операции", ".//div[contains(@class,'trs_date')]");
            Summ = new DataColumn("Сумма", ".//div[contains(@class,'trs_sum')]");
            BalanceArter = new DataColumn("Остаток на карте",null);
            Location = new DataColumn("Место", "//span[contains(@class,'trs_country')]");
            DateProceed = new DataColumn("Дата проведения", "//span[contains(@class,'idate')]");
        }       
    }
}
