using System;

namespace ParserCore
{
    public class CardOperation
    {
        public int RowNumber { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public decimal Summ { get; set; } 
        public string Location { get; set; }
        public DateTime ProcessDate { get; set; }
        public decimal BalanceAfter { get; set; }
        public string Category { get; set; }
    }
}
