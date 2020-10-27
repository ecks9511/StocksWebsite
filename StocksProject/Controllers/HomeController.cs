using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceStack;
using StocksProject.Models;

namespace StocksProject.Controllers
{
    public class HomeController : Controller
    {
        /*private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }*/

        private IAvDataRepository _quotesService;

        public HomeController(IAvDataRepository quotesService)
        {
            _quotesService = quotesService;
        }

        [HttpGet]
        public IEnumerable<AvMonthlyQuoteData> Get()
        {
            return _quotesService.GetAllQuotes();
        }

        [HttpPost]
        public void Post([FromBody] string symbol)
        {
            _quotesService.AddQuote(symbol);
        }

        [HttpDelete("{id}")]
        public void Delete(string symbol)
        {
            _quotesService.DeleteQuote(symbol);
        }

        public IActionResult Index()
        {
            List<string> stockSymbols = new List<string>() {"IBM"};
            foreach (var s in stockSymbols)
            {
                _quotesService.AddQuote(s);
            }

            return View(_quotesService);
        }

        public IActionResult Stocks()
        {
            return View();
        }

        public IActionResult Crypto()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
