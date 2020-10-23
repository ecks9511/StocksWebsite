using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using ServiceStack;
using StocksProject.Models;

namespace StocksProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Your stock profile";

            //Parent class for parsing down to nested values
            var avData = new AlphaVantage();

                //Send string to api and get back CSV file in string
                var response = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=GOOG&datatype=csv&apikey=QBT57BYWKH947L5Z"
                        .GetStringFromUrl();
            try
            {
                //Parse data from CSV string to new variable
                var allData = response.FromCsv<List<Quote>>().ToList();

                //Put parsed data into AlphaVantage object for use in view
                avData.Entries = allData.ToList();
            }
            catch
            {
                avData.ErrorMessage = "ERROR : Information parsed incorrectly";
            }

            return View(avData);
        }
        public IActionResult StockPage()
        {


            return View();
        }


        public IActionResult Privacy()
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
