using System;
namespace JobSityChat.Core.MBQueues
{
    public interface IStockQueueProducer
    {
        void RequestStock(string stockValue);
    }
}
