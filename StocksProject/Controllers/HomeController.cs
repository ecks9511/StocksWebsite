using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChartJSCore.Helpers;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using ServiceStack;
using StocksProject.Models;
using ChartJSCore.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

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
            //Make container for all monthly quotes
            AVAllData allData = new AVAllData();

            //Stocks to grab with their corresponding full names
            List<String> stockNames = new List<string> { "GOOG", "TSLA", "MSFT","AAPL","AMZN"};
            string apiKey = "3Y8HPMEXDL322QUV";

            //Loop through each stock, grab the quote and add it to allData object
            for (int i = 0; i < stockNames.Count; i++)
                allData.allMonthlyData.Add(SingleStock(stockNames[i]));

            return View(allData);
        }

        public IActionResult Stocks()
        {
            //Make container for all monthly quotes
            AVAllData allData = new AVAllData();

            allData.allMonthlyData.Add(SingleStock("IBM"));

            return View(allData);
        }

        [HttpGet]
        public IActionResult GetStock(AVAllData allData, string symbol)
        {

            allData.allMonthlyData.Add(SingleStock(symbol));

            return View("Stocks", allData);
        }

        public AVMonthlyQuoteData SingleStock(string symbol)
        {
            //Lists for graphing
            List<double> openPrices = new List<double>();
            List<String> dates = new List<String>();

            string apiKey = "08158IP9AW9WZIZW"; //"3Y8HPMEXDL322QUV"; //"QBT57BYWKH947L5Z";

            //Parent class for parsing down to nested values
            var curMonthlyQuote = new AVMonthlyQuoteData();

            //Send string to api and get back CSV file in string
            var response =
                $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY&symbol={symbol}&datatype=csv&apikey={apiKey}"
                    .GetStringFromUrl();

                Console.WriteLine(response);
                try
                {
                    //Parse data from CSV string to new variable
                    var allMonthly = response.FromCsv<List<MonthlyQuote>>().ToList();

                    //Add all of the prices to parent object for later graphing
                    for (int i = 0; i < allMonthly.Count; i++)
                    {
                        openPrices.Add(allMonthly[i].Open);
                        dates.Add(allMonthly[i].Timestamp.ToString("MM-dd-yyyy"));

                    }


                    //Adding graph data
                    curMonthlyQuote.EntryOpenPrices = openPrices;
                    curMonthlyQuote.EntryDateTime = dates;

                    //Adding names to parent class
                    curMonthlyQuote.Symbol = symbol;


                    //Put parsed data into AlphaVantage object for use in view
                    curMonthlyQuote.Entries = allMonthly.ToList();
                }
                catch
                {
                    curMonthlyQuote.ErrorMessage = "ERROR : Information parsed incorrectly";
                }

                return curMonthlyQuote;
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
