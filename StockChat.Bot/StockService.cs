using CsvHelper;
using StockChat.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockChat.Bot
{
    public class StockService
    {
        public static async Task<ChatMessage> GetStock(string stockCode)
        {
            using HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(stream);
                var splited = new CsvReader(reader, CultureInfo.InvariantCulture);
                var stock = splited.GetRecords<Stock>().First();

                var res = new ChatMessage();

                if(stock.Close != "N/D")
                {
                    res = new ChatMessage
                    {
                        Message = $"{stock.Symbol} quote is ${stock.Close} per share",
                        IsSuccessful = true
                    };
                }
                else
                {
                    res = new ChatMessage
                    {
                        Message = $"Stock not found",
                        IsSuccessful = false
                    };
                }

                return res;

            }
            else
            {
                var res = new ChatMessage
                {
                    Message = "Error calling stock API",
                    IsSuccessful = false
                };

                return res;
            }
        }
    }
}
