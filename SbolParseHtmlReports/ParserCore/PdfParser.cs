using System;

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
            throw new NotImplementedException();
        }
    }
}
