using System;
using System.ComponentModel.DataAnnotations;

namespace JobSityChat.Core.Entities
{
    public class UserMessage
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}