using System;
namespace JobSityChat.Api.Models
{
    public class MessageViewModel
    {
        public string UserName { get; set; }
        public string UserMessage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}