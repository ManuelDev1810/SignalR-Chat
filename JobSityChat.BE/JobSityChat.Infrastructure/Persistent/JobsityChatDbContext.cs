using System;
using JobSityChat.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobSityChat.Infrastructure.Persistent
{
    public class JobsityChatDbContext : DbContext
    {

        public JobsityChatDbContext(DbContextOptions<JobsityChatDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<UserMessage> UserMessages { get; set; }
    }
}
