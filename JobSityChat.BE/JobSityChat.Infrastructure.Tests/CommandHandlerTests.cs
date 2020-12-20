using System;
using JobSityChat.Infrastructure.Services.Handlers;
using Xunit;

namespace JobSityChat.Infrastructure.Tests
{
    public class CommandHandlerTests
    {
        [Theory]
        [InlineData("/", false)]
        [InlineData("dfgfdg", false)]
        [InlineData("/dada", true)]
        public void IsCommand_ShouldReturnFalseWhenDontStartWithSlashAndAnotherCharacter
            (string text, bool expected)
        {
            CommandHandler commandHandler = new CommandHandler();

            //Arrange
            

            //Act
            bool actual = commandHandler.IsCommand(text);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("/", false)]
        [InlineData("dfgfdg", false)]
        [InlineData("/dada", true)]
        public void IsCommand_ShouldReturnTrueIfStartWithSlashAndLengthIsGreatherThanOne
            (string text, bool expected)
        {
            CommandHandler commandHandler = new CommandHandler();

            //Arrange

            //Act
            bool actual = commandHandler.IsCommand(text);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("/stock=cool", "stock", "cool")]
        [InlineData("/Job=Sity", "job", "sity")]
        [InlineData("/let's=go", "let's", "go")]
        public void GetValues_ShouldReturnCommandAndValue
            (string test, string expectedCommand, string exptecedValue)
        {
            CommandHandler commandHandler = new CommandHandler();

            //Arrange

            //Act and Assert
            commandHandler.GetValues(test, (command, value) => {
                //Assert Command
                Assert.Equal(expectedCommand, command);

                //Assert Value
                Assert.Equal(exptecedValue, value);
            });
        }
    }
}
