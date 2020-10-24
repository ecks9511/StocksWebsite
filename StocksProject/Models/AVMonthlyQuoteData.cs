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

        public List<double> EntryOpenPrices { get; set; }
        public List<String> EntryDateTime { get; set; }
        public string ErrorMessage { get; set; } = "";

    }
}
