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

        public async Task<IReadOnlyList<UserMessage>> GetMessages()
        {
            var messages = await _context.UserMessages.ToListAsync();
            messages = messages.OrderByDescending(t => t.CreatedAt).ToList();
            return messages;
        }

        public async Task<IReadOnlyList<UserMessage>> GetLast50Messages()
        {
            var messages = await _context.UserMessages.ToListAsync();
            messages = messages.TakeLast(50).ToList();
            messages = messages.OrderByDescending(t => t.CreatedAt).ToList();
            return messages;
        }

    }
}
