using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JobSityChat.Infrastructure.Persistent
{
    public class JobSityChatDbContextFactory : IDesignTimeDbContextFactory<JobsityChatDbContext>
    {
    
        public JobsityChatDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<JobsityChatDbContext>();
            optionsBuilder.UseSqlite("Data Source=JobSityChat.db");

            return new JobsityChatDbContext(optionsBuilder.Options);
        }
    }
}
