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

namespace JobSityChat.Api.MBQueues
{
    public class StockQueueConsumer : BackgroundService, IStockQueueConsumer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly IHubContext<ChatHub> _chatHub;

        public StockQueueConsumer(IHubContext<ChatHub> chatHub)
        {
            //Opening the RabbitMQ connection
            _factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
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
                    //sending the message through SignalR
                    string response = $"{stock.Symbol.ToUpper()} quote is {stock.Close:F} per share";
                    await SendResponse(response);
                }
                else
                {
                    //sending the message through SignalR
                    string response = "We couldn't find any stock with that code";
                    await SendResponse(response);
                }

                _channel.BasicAck(e.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: MBQueueConstants.STOCK_QUEUE_RESPONSE,
                autoAck: true,
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

        public async Task SendResponse(string response)
        {
            await _chatHub.Clients.All.SendAsync(ChatHubConstants.METHOD_CHAT_NAME, new MessageViewModel() {
                UserName = "StockBot",
                UserMessage = response,
                CreatedAt = DateTime.Now
            });
        }
    }
}
