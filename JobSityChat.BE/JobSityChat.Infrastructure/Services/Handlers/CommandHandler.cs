using System;
using JobSityChat.Core.Handlers.Interfaces;

namespace JobSityChat.Infrastructure.Services.Handlers
{
    public class CommandHandler : ICommandHandler
    {
        public void GetValues(string userCommad, Action<string, string> action)
        {
            string commandArguments = userCommad.Trim().Substring(1).ToLower();

            string[] arguments = commandArguments.Split("=");

            string command = string.Empty;
            string value = string.Empty;

            if (arguments.Length < 2)
            {
                command = arguments[0];
            }
            else
            {
                command = arguments[0];
                value = arguments[1];
            }

            action(command, value);
        }

        public bool IsCommand(string command)
        {
            if (command.StartsWith("/") && command.Length > 1) return true;

            return false;
        }
    }
}
