using System;
namespace JobSityChat.Core.Handlers.Interfaces
{
    public interface ICommandHandler
    {
        bool IsCommand(string command);
        void GetValues(string userCommad, Action<string, string> action);
    }
}
