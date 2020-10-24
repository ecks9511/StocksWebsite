using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksProject.Models
{
    public class AVDailyQuoteData 
    {
        public string Name { get; set; } = "";
        public List<DailyQuote> Entries { get; set; } = new List<DailyQuote>();
        public string ErrorMessage { get; set; } = "";

        public ChartJSCore.Models.Chart FrontChart { get; set; }
    }
}
