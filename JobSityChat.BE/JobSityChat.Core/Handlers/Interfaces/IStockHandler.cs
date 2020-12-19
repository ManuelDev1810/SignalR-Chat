using System;
using System.Threading.Tasks;
using JobSityChat.Core.Entities;

namespace JobSityChat.Core.Handlers.Interfaces
{
    public interface IStockHandler
    {
        bool IsStockCommand(string stockCommand);
        Task<Stock> GetStock(string stockValue);
    }
}
