using System;
using System.Threading.Tasks;
using JobSityChat.Core.Entities;

namespace JobSityChat.Core.Handlers.Interfaces
{
    public interface IStockHandler
    {
        Task<Stock> GetStockData(string stockValue);
    }
}
