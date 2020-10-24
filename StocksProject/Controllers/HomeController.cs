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
            //Stocks to grab with their corresponding full names
            List<String> stockNames = new List<string> { "GOOG", "TSLA", "MSFT", "AMZN", "AAPL" };
            List<String> stockNamesFull = new List<string> { "Google", "Tesla", "Microsoft", "Amazon", "Apple" };
            int curName = 0;

            //API keys for calls
            String apiKey = "3Y8HPMEXDL322QUV"; //String apiKey = "QBT57BYWKH947L5Z";
            
            //Lists for storing all of the data objects to send to the view
            AVAllData allData = new AVAllData();
            List<AVMonthlyQuoteData> allMonthlyQuoteData = new List<AVMonthlyQuoteData>();

            //Loop through and grab all of the monthly stock info
            foreach (var curStock in stockNames)
            {

                //Lists for graphing
                List<double> openPrices = new List<double>();
                List<String> dates = new List<String>();
                //Parent class for parsing down to nested values
                var avData = new AVMonthlyQuoteData();

                //Send string to api and get back CSV file in string
                var response =
                    $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY&symbol={curStock}&datatype=csv&apikey={apiKey}"
                        .GetStringFromUrl();

                Console.WriteLine(response);
                try
                {
                    //Parse data from CSV string to new variable
                    var allMonthly = response.FromCsv<List<MonthlyQuote>>().ToList();

                    //Add name for specific stock 
                    avData.Name = stockNamesFull.ElementAt(curName);
                    avData.Symbol = stockNames.ElementAt(curName);

                    //Add all of the prices to parent object for later graphing
                    for (int i = 0; i < allMonthly.Count; i++)
                    {
                        openPrices.Add(allMonthly[i].Open);
                        dates.Add(allMonthly[i].Timestamp.ToString("MM-dd-yyyy"));

                    }
                    //So dates arent backwards
                    dates.Reverse();

                    //Adding graph data
                    avData.EntryOpenPrices = openPrices;
                    avData.EntryDateTime = dates;

                    //Put parsed data into AlphaVantage object for use in view
                    avData.Entries = allMonthly.ToList();
                }
                catch
                {
                    avData.ErrorMessage = "ERROR : Information parsed incorrectly";
                }

                //Add to parent class
                allMonthlyQuoteData.Add(avData);

                //Increment name counter
                curName++;
            }

            //Put into parent object
            allData.allMonthlyData = allMonthlyQuoteData;

            return View(allData);
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
