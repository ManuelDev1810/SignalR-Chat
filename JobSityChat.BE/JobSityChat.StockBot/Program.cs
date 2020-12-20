using System;
using System.Threading;
using JobSityChat.Infrastructure.Services.Handlers;
using JobSityChat.StockBot.MBQueues;

namespace JobSityChat.StockBot
{
    class Program
    {
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            //Setting up the producer
            StockResponseQueueProducer producer = new ();
            StockHandler stockHandler = new();

            //Setting up the consumer
            StockRequestQueueConsumer consumer = new (stockHandler, producer);

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
