using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobSityChat.Api.Models;
using JobSityChat.Core.Entities;
using JobSityChat.Core.Handlers.Interfaces;
using JobSityChat.Core.MBQueues;
using JobSityChat.Core.Persistent;
using JobSityChat.Core.Repository.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace JobSityChat.Api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ICommandHandler _commandHandler;
        private readonly IStockQueueProducer _stockQueueProducer;
        private readonly IUserMessageRepository _userMessageRepository;

        public ChatHub(ICommandHandler commandHandler, IStockQueueProducer stockQueueProducer,
                       IUserMessageRepository userMessageRepository)
        {
            _commandHandler = commandHandler;
            _stockQueueProducer = stockQueueProducer;
            _userMessageRepository = userMessageRepository;
        }

        public async Task SendMessage(MessageViewModel message)
        {
            if (_commandHandler.IsCommand(message.Message))
            {
                //Send message to the queqe
                _commandHandler.GetValues(message.Message, (command, value) =>
               {
                   if (command == CommandConstants.Stock)
                   {
                       // send message to stock queue
                       _stockQueueProducer.RequestStock(value);
                   }
               });

            } else
            {
                //Saving the message to the database
                await SaveMessage(message);

                //Getting the messages
                await Clients.All.SendAsync(ChatHubConstants.METHOD_CHAT_NAME, message);
            }

        }

        private async Task SaveMessage(MessageViewModel model)
        {
            await _userMessageRepository.AddAsync(new UserMessage
            {
                Message = model.Message,
                ID = model.ID,
                CreatedAt = model.CreatedAt,
                Name = model.Name
            });
        }
    }
}