using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StockChat.Bot
{
    public class Worker : BackgroundService
    {
        readonly IBus _bus;

        public Worker(IBus bus)
        {
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //var stockMsg = await StockService.GetStock("aapl.us");
                ////await _bus.Publish(new Message { Text = $"The time is {DateTimeOffset.Now}" });
                //await _bus.Publish(new Message { Text = stockMsg });

                //await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
