using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksProject.Models
{
    public class MonthlyQuote
    {
        public DateTime Timestamp { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double AdjustedClose { get; set; }
        public double Volume { get; set; }
        public double DividendAmount { get; set; }
    }
}
