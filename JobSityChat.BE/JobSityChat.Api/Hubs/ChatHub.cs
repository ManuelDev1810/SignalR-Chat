using System;
using System.Threading.Tasks;
using JobSityChat.Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace JobSityChat.Api.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(MessageViewModel message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message.UserName, message.UserMessage);
        }
    }
}