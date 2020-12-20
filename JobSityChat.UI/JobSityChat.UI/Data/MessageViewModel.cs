using System;
namespace JobSityChat.UI.Data
{
    public class MessageViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
