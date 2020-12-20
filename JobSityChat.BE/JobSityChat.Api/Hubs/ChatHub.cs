using System;
using System.Threading.Tasks;
using JobSityChat.Api.Models;
using JobSityChat.Core.Handlers.Interfaces;
using JobSityChat.Core.MBQueues;
using JobSityChat.Core.Persistent;
using Microsoft.AspNetCore.SignalR;

namespace JobSityChat.Api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ICommandHandler _commandHandler;
        private readonly IStockQueueProducer _stockQueueProducer;

        public ChatHub(ICommandHandler commandHandler, IStockQueueProducer stockQueueProducer)
        {
            _commandHandler = commandHandler;
            _stockQueueProducer = stockQueueProducer;
        }

        public async Task SendMessage(MessageViewModel message)
        {
            if (_commandHandler.IsCommand(message.UserMessage))
            {
                //Send message to the queqe
                _commandHandler.GetValues(message.UserMessage, (command, value) =>
               {
                   if (command == CommandConstants.Stock)
                   {
                       // send message to stock queue
                       _stockQueueProducer.RequestStock(value);
                   }
               });

            } else
            {
                message = new MessageViewModel { UserMessage = "dfsdf", UserName = "ssf", CreatedAt = DateTime.Now };
                //Saving the message to the database
                await Clients.All.SendAsync(ChatHubConstants.METHOD_CHAT_NAME, message);
            }
        }


    }
}