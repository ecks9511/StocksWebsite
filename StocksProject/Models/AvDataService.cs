using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StocksProject.Models
{
    public interface IAvDataRepository
    {
        List<AvMonthlyQuoteData> GetAllQuotes();
        List<AvMonthlyQuoteData> AddQuote(string symbol);
        List<AvMonthlyQuoteData> DeleteQuote(string symbol);
    }

}
