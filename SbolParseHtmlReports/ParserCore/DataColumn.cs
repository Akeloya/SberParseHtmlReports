namespace ParserCore
{
    public class DataColumn
    {
        public string Name { get; set; }
        public string XPath { get; set; }

        public DataColumn(string name, string xPath)
        {
            Name = name;
            XPath = xPath;
        }
    }
}
