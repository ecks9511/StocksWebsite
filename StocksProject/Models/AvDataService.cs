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
        void AddQuote(string symbolName, IHostEnvironment env);
        void AddCrypto(string cryptoName, IHostEnvironment env);
        string GetQuotes(IHostEnvironment env);
        string GetQuotesLanding(IHostEnvironment env);

        string GetCrypto(IHostEnvironment env);
        void DeleteQuote(string symbolName, IHostEnvironment env);

        void DeleteCrypto(string cryptoName, IHostEnvironment env);
    }

}
