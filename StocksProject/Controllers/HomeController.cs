using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ServiceStack;
using StocksProject.Models;
using Microsoft.Extensions.Hosting;
namespace StocksProject.Controllers
{
    public class HomeController : Controller
    {
        /*private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }*/

        public IAvDataRepository _quotesService;
        private IHostEnvironment _env;

        public HomeController(IAvDataRepository quotesService, IHostEnvironment env)
        {
            _quotesService = quotesService;
            _env = env;
        }

        [HttpPost]
        public IActionResult DeleteStock(string symbolName)
        {
            _quotesService.DeleteQuote(symbolName,_env);
            ViewData["AllStocks"] = GetAll();
            return View("Stocks");
        }


        [HttpPost]
        public IActionResult AddStock(IFormCollection form)
        {
            string stockName = Convert.ToString(form["symbolName"]);
            List<string> names = new List<string>() { stockName };
            _quotesService.AddQuotes(names, _env);
            ViewData["AllStocks"] = GetAll();
            return View("Stocks");
        }

        public string GetAll()
        {
            return _quotesService.GetQuotes(_env);
        }

        public string GetAllLanding()
        {
            return _quotesService.GetQuotesLanding(_env);
        }


        public IActionResult Index()
        {
            ViewData["AllStocksLanding"] = GetAllLanding();
            return View();
        }

        public IActionResult Stocks()
        {
            ViewData["AllStocks"] = GetAll();
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
