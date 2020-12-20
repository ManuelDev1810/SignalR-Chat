using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JobSityChat.Core.Entities;
using JobSityChat.Core.Handlers.Interfaces;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace JobSityChat.Infrastructure.Services.Handlers
{
    public class StockHandler : IStockHandler
    {
        public async Task<Stock> GetStockData(string stockValue)
        {
            //Constructing the url
            string url = $"https://stooq.com/q/l/?s={stockValue}&f=sd2t2ohlcv&h&e=csv";

            //Maquint the request
            HttpClient client = new HttpClient();
            Stream content = await client.GetStreamAsync(url);

            //Parsing the Csv with TinyCsvParser library
            CsvParser<Stock> stockParser = new CsvParser<Stock>(new CsvParserOptions(true, ','),
               new StockCsvMapping());

            //Reading the stock data
            var results = stockParser.ReadFromStream(stream: content, Encoding.UTF8).ToList();
            return results.FirstOrDefault()?.Result;
        }
    }

    public class StockCsvMapping : CsvMapping<Stock>
    {
        public StockCsvMapping() : base()
        {
            MapProperty(0, t => t.Symbol);
            MapProperty(1, t => t.Date);
            MapProperty(2, t => t.Time);
            MapProperty(3, t => t.Open);
            MapProperty(4, t => t.High);
            MapProperty(5, t => t.Low);
            MapProperty(6, t => t.Close);
            MapProperty(7, t => t.Volume);
        }
    }
}
