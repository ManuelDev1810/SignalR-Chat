using System;
using System.Text;
using JobSityChat.Core.MBQueues;
using JobSityChat.Core.Persistent;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace JobSityChat.Api.MBQueues
{
    public class StockQueueProducer : IStockQueueProducer
    {
        protected readonly IConfiguration _configuration;
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;

        public StockQueueProducer(IConfiguration configuration)
        {
            _configuration = configuration;

            //Opening the RabbitMQ connection
            _factory = new() { HostName = "amqp://guest:guest@localhost:5672" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void RequestStock(string stockValue)
        {
            _channel.QueueDeclare(queue: MBQueueConstants.STOCK_QUEUE_REQUEST,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = Encoding.UTF8.GetBytes(stockValue);

            _channel.BasicPublish(exchange: "", routingKey: MBQueueConstants.STOCK_QUEUE_REQUEST, body: message, basicProperties: null);
        }
    }
}
