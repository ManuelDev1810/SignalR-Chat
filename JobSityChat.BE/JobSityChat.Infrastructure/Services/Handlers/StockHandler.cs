using System;
using System.Threading.Tasks;
using JobSityChat.Core.Entities;
using JobSityChat.Core.Handlers.Interfaces;

namespace JobSityChat.Infrastructure.Services.Handlers
{
    public class StockHandler : IStockHandler
    {
        public StockHandler()
        {
        }

        public Task<Stock> GetStock(string stockValue)
        {
            throw new NotImplementedException();
        }

        public bool IsStockCommand(string stockCommand)
        {
            if (stockCommand.StartsWith("/stock") && stockCommand.Length > 1) return true;

            return false;
        }
    }
}
