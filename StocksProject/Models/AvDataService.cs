using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace StocksProject.Models
{
    public interface IAvDataRepository
    {
        void AddQuotes(string symbolName, IHostEnvironment env);
        string GetQuotes(IHostEnvironment env);
        string GetQuotesLanding(IHostEnvironment env);
        void DeleteQuote(string symbolName, IHostEnvironment env);
    }

}
