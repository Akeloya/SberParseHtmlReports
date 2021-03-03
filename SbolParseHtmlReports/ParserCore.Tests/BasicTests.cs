using System;
using System.IO;
using Xunit;

namespace ParserCore.Tests
{
    public class BasicTests
    {
        private const string _fileNames = "D:\\TestsData";
        private const string _outFileName = "result.csv";
        [Fact]
        public void TestParse()
        {
            var pathWithDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _outFileName);
            var files = Directory.GetFiles(_fileNames, "*.html");

            foreach(var file in files)
            {
                var parser = new Parser(file, pathWithDir,new DataSet(),';');
                parser.RunParse();
            }
            
        }
    }
}
