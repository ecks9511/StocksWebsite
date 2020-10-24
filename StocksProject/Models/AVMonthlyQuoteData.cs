using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksProject.Models
{
    public class AVMonthlyQuoteData
    {

        public string Name { get; set; } = "";
        public string Symbol { get; set; } = "";
        public List<MonthlyQuote> Entries { get; set; } = new List<MonthlyQuote>();
        public string ErrorMessage { get; set; } = "";

        public ChartJSCore.Models.Chart FrontChart { get; set; }
    }
}
