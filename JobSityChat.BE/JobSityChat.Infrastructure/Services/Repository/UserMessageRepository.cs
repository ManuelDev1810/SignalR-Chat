using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobSityChat.Core.Entities;
using JobSityChat.Core.Repository.Interfaces;
using JobSityChat.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace JobSityChat.Infrastructure.Services.Repository
{
    public class UserMessageRepository : IUserMessageRepository
    {
        private readonly JobsityChatDbContext _context;
        public UserMessageRepository(JobsityChatDbContext context)
        {
            _context = context;
        }

        public async Task<UserMessage> AddAsync(UserMessage entity)
        {
            _context.UserMessages.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IReadOnlyList<UserMessage>> GetLast50Messages()
        {
            return await _context.UserMessages.OrderBy(t => t.CreatedAt).TakeLast(50).ToListAsync();
        }
    }
}
