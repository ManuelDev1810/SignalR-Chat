using System;
using System.IO;
using System.Threading;
using JobSityChat.Infrastructure.Services.Handlers;
using JobSityChat.StockBot.MBQueues;
using Microsoft.Extensions.Configuration;

namespace JobSityChat.StockBot
{
    class Program
    {
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            //Reading the configuration file
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            //Setting up the producer
            StockResponseQueueProducer producer = new (configuration);
            StockHandler stockHandler = new();

            //Setting up the consumer
            StockRequestQueueConsumer consumer = new (configuration, stockHandler, producer);

            //Start to listen for the Api
            consumer.Run();

            Console.WriteLine("Waiting for request to look for stocks");

            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);

            _closing.WaitOne();
        }

        private static void OnExit(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Exit");
            _closing.Set();
        }
    }
}
