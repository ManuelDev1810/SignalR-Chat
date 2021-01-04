using System;
using System.Text;
using JobSityChat.Core.Entities;
using JobSityChat.Core.Handlers.Interfaces;
using JobSityChat.Core.Persistent;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace JobSityChat.StockBot.MBQueues
{
    public class StockRequestQueueConsumer
    {
        private readonly StockResponseQueueProducer _producer;
        private readonly IStockHandler _stockHandler;

        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;

        protected readonly IConfiguration _configuration;

        public StockRequestQueueConsumer(IConfiguration configuration, IStockHandler stockHandler, StockResponseQueueProducer producer)
        {
            _configuration = configuration;

            //Opening the RabbitMQ connection
            _factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"]
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            //Initializing the stock handler and the stock producer
            _stockHandler = stockHandler;
            _producer = producer;
        }

        public void Run()
        {
            _channel.QueueDeclare(queue: MBQueueConstants.STOCK_QUEUE_REQUEST,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            //Consuming the request of the API
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, e) =>
            {
                //Getting the stock value
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine("Received ", message);

                //Getting the stock data
                Stock stock = await _stockHandler.GetStockData(message);
                var result = JsonConvert.SerializeObject(stock);

                //Calling the producer so the API can receive the data
                _producer.SendStockData(stock);
            };

            _channel.BasicConsume(queue: MBQueueConstants.STOCK_QUEUE_REQUEST,
                autoAck: true,
                consumer: consumer);
        }
    }
}
