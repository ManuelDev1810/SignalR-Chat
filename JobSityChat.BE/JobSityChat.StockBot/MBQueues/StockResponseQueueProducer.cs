using System;
using System.Text;
using JobSityChat.Core.Entities;
using JobSityChat.Core.Persistent;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace JobSityChat.StockBot.MBQueues
{
    public class StockResponseQueueProducer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;

        protected readonly IConfiguration _configuration;

        public StockResponseQueueProducer(IConfiguration configuration)
        {
            _configuration = configuration;

            //Opening the RabbitMQ connection
            _factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"]
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendStockData(Stock stock)
        {
            _channel.QueueDeclare(queue: MBQueueConstants.STOCK_QUEUE_RESPONSE,
               durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null);

            var stockInformation = JsonConvert.SerializeObject(stock);
            var body = Encoding.UTF8.GetBytes(stockInformation);

            _channel.BasicPublish(exchange: "", routingKey: MBQueueConstants.STOCK_QUEUE_RESPONSE, body: body, basicProperties: null);

            Console.WriteLine("Send", stockInformation);
        }
    }
}
