using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;

namespace StocksProject.Models
{
    public class AvDataRepository : IAvDataRepository
    {
        List<AvMonthlyQuoteData> _quotes = new List<AvMonthlyQuoteData>();
        public List<AvMonthlyQuoteData> AddQuote(string symbol)
        {
            //Lists for graphing
            List<double> openPrices = new List<double>();
            List<String> dates = new List<String>();

            string apiKey = "08158IP9AW9WZIZW"; //"3Y8HPMEXDL322QUV"; //"QBT57BYWKH947L5Z";

            //Parent class for parsing down to nested values
            var curMonthlyQuote = new AvMonthlyQuoteData();

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

            _quotes.Add(curMonthlyQuote);

            return _quotes;
        }

        public List<AvMonthlyQuoteData> DeleteQuote(string symbol)
        {
            _quotes.RemoveAll(x => x.Symbol == symbol);
            return _quotes;
        }

        public List<AvMonthlyQuoteData> GetAllQuotes()
        {
            return _quotes;
        }
    }

    public class AvMonthlyQuoteAll
    {
        public List<AvMonthlyQuoteData> Entries { get; set; } = new List<AvMonthlyQuoteData>();
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
