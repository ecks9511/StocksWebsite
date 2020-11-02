using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServiceStack;
using System.Web;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting;


namespace StocksProject.Models
{
    public class AvDataRepository : IAvDataRepository
    {
        public void AddQuotes(string symbolName, IHostEnvironment env)
        {
            if (!symbolName.IsNullOrEmpty())
            {
            List<AvMonthlyQuoteData> allData = new List<AvMonthlyQuoteData>();
            string apiKey = "08158IP9AW9WZIZW"; //"3Y8HPMEXDL322QUV"; //"QBT57BYWKH947L5Z";
            Boolean alreadyAdded = false;

                //Parent class for parsing down to nested values
                var curMonthlyQuote = new AvMonthlyQuoteData();

                //Lists for graphing
                List<double> openPrices = new List<double>();
                List<String> dates = new List<String>();

                //Send string to api and get back CSV file in string
                var response =
                    $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY&symbol={symbolName}&datatype=csv&apikey={apiKey}"
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
                    curMonthlyQuote.Symbol = symbolName;


                    //Put parsed data into AlphaVantage object for use in view
                    curMonthlyQuote.Entries = allMonthly.ToList();
                }
                catch
                {
                    curMonthlyQuote.ErrorMessage = "ERROR : Information parsed incorrectly";
                }

                allData.Add(curMonthlyQuote);

                    var path = env.ContentRootPath + "\\DataAccess\\stocks2.txt";

                if (File.Exists(path))
                {
                    var jsonData = File.ReadAllText(path);
                    var convertedQuoteData = JsonConvert.DeserializeObject<List<AvMonthlyQuoteData>>(jsonData)
                                             ?? new List<AvMonthlyQuoteData>();

                    foreach (var parsedStock in convertedQuoteData)
                    {
                            if (parsedStock.Symbol == symbolName)
                                alreadyAdded = true;
                    }

                    if (alreadyAdded == false)
                    {
                        convertedQuoteData.AddRange(allData);

                        // Update json data string
                        jsonData = JsonConvert.SerializeObject(convertedQuoteData);
                        System.IO.File.WriteAllText(path, string.Empty);
                        System.IO.File.WriteAllText(path, jsonData);
                    }

                }
                else
                    File.AppendAllText(path, JsonConvert.SerializeObject(allData));
            }

        }
        public string GetQuotes(IHostEnvironment env)
        {
            var content = System.IO.File.ReadAllText(env.ContentRootPath + "\\DataAccess\\stocks2.txt");
            return content;
        }
        public string GetQuotesLanding(IHostEnvironment env)
        {
            var content = System.IO.File.ReadAllText(env.ContentRootPath + "\\DataAccess\\stocks.txt");
            return content;
        }

        public void DeleteQuote(string symbolName, IHostEnvironment env)
        {
            var path = env.ContentRootPath + "\\DataAccess\\stocks2.txt";

            if (File.Exists(path))
            {
                var jsonData = File.ReadAllText(path);
                var convertedQuoteData = JsonConvert.DeserializeObject<List<AvMonthlyQuoteData>>(jsonData)
                                         ?? new List<AvMonthlyQuoteData>();

                convertedQuoteData.RemoveAll(x => x.Symbol == symbolName);

                    // Update json data string
                var newJsonData = JsonConvert.SerializeObject(convertedQuoteData);
                System.IO.File.WriteAllText(path, string.Empty);
                System.IO.File.WriteAllText(path, newJsonData);
            }

        }

    }


    public class AvMonthlyQuoteData
    {
        public string Symbol { get; set; } = "";
        public List<MonthlyQuote> Entries { get; set; } = new List<MonthlyQuote>();

        public List<double> EntryOpenPrices { get; set; } = new List<double>();
        public List<String> EntryDateTime { get; set; } = new List<String>();
        public string ErrorMessage { get; set; } = "";

    }

    public class MonthlyQuote
    {
        public DateTime Timestamp { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double AdjustedClose { get; set; }
        public double Volume { get; set; }
        public double DividendAmount { get; set; }

    }


}
