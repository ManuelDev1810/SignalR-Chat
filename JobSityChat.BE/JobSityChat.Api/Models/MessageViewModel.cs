using System;
namespace JobSityChat.Api.Models
{
    public class MessageViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}