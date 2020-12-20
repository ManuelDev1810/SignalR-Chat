using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobSityChat.Core.Entities;

namespace JobSityChat.Core.Repository.Interfaces
{
    public interface IUserMessageRepository
    {
        Task<UserMessage> AddAsync(UserMessage entity);
        Task<IReadOnlyList<UserMessage>> GetLast50Messages();
    }
}
