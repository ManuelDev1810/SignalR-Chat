using System;
namespace JobSityChat.Core.MBQueues
{
    public interface IStockQueueConsumer
    {
        void Run();
    }
}
