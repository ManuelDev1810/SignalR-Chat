using System;
using System.Threading.Tasks;
using JobSityChat.Api.Models;
using JobSityChat.Core.Handlers.Interfaces;
using JobSityChat.Core.Persistent;
using Microsoft.AspNetCore.SignalR;

namespace JobSityChat.Api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IStockHandler _stockHandler;

        public ChatHub(IStockHandler stockHandler)
        {
            _stockHandler = stockHandler;
        }

        public async Task SendMessage(MessageViewModel message)
        {
            if (_stockHandler.IsStockCommand(message.UserMessage))
            {
               //Send message to the queqe


            } else
            {
                //Saving the message to the database
            }

            await Clients.All.SendAsync(JobSityChatHubConstant.METHOD_CHAT_NAME, message.UserName, message.UserMessage);
        }
    }
}