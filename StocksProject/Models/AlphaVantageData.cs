using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksProject.Models
{
    public class AlphaVantageData 
    {
        public string Name { get; set; } = "";
        public List<Quote> Entries { get; set; } = new List<Quote>();
        public string ErrorMessage { get; set; } = "";
    }
}
