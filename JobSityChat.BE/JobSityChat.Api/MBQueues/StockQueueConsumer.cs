using System;
using JobSityChat.Core.MBQueues;
using RabbitMQ.Client;
using Microsoft.AspNetCore.SignalR;
using JobSityChat.Api.Hubs;
using JobSityChat.Core.Persistent;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using JobSityChat.Core.Entities;
using System.Threading.Tasks;
using JobSityChat.Api.Models;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace JobSityChat.Api.MBQueues
{
    public class StockQueueConsumer : BackgroundService, IStockQueueConsumer
    {
        protected readonly IConfiguration _configuration;
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IHubContext<ChatHub> _chatHub;

        public StockQueueConsumer(IHubContext<ChatHub> chatHub, IConfiguration configuration)
        {
            _configuration = configuration;

            //Opening the RabbitMQ connection
            _factory = new ConnectionFactory
            {
                Uri = new Uri(_configuration["RabbitMQ:Host"])
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: MBQueueConstants.STOCK_QUEUE_RESPONSE,
                durable: true,
                exclusive: false,
                autoDelete: false);
            _chatHub = chatHub;
        }

        public void Run()
        {
            //Consuming response from the StockBot
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stock = JsonConvert.DeserializeObject<Stock>(message);

                if (stock is not null)
                {
                    //sending the stock information through SignalR
                    string response = $"{stock.Symbol.ToUpper()} quote is {stock.Close:F} per share";
                    await SendResponse(response);
                }
                else
                {
                    //sending a message indicating that couldn't find a response
                    string response = ChatHubConstants.STOCK_NOT_FOUND;
                    await SendResponse(response);
                }

                _channel.BasicAck(e.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: MBQueueConstants.STOCK_QUEUE_RESPONSE,
                autoAck: false,
                consumer: consumer);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            Run();
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

        public async Task SendResponse(string message)
        {
            await _chatHub.Clients.All.SendAsync(ChatHubConstants.METHOD_CHAT_NAME,
                new MessageViewModel { Name = "StockBot", Message = message, CreatedAt = DateTime.Now}
            );
        }
    }
}
