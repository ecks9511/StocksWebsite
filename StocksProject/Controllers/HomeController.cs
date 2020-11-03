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
        public IActionResult AddStock(string symbolName)
        {
            _quotesService.AddQuote(symbolName, _env);
            ViewData["AllStocks"] = GetAll();
            return View("Stocks");
        }
        [HttpPost]
        public IActionResult DeleteCrypto(string cryptoName)
        {
            _quotesService.DeleteCrypto(cryptoName, _env);
            ViewData["AllCrypto"] = GetAllCrypto();
            return View("Crypto");
        }


        [HttpPost]
        public IActionResult AddCrypto(string cryptoName)
        {
            _quotesService.AddCrypto(cryptoName, _env);
            ViewData["AllCrypto"] = GetAllCrypto();
            return View("Crypto");
        }
        [HttpGet]
        public string GetAll()
        {
            return _quotesService.GetQuotes(_env);
        }
        [HttpGet]
        public string GetAllLanding()
        {
            return _quotesService.GetQuotesLanding(_env);
        }
        [HttpGet]
        public string GetAllCrypto()
        {
            return _quotesService.GetCrypto(_env);
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
            ViewData["AllCrypto"] = GetAllCrypto();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
