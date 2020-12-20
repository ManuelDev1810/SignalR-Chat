using System;
using JobSityChat.Infrastructure.Services.Handlers;
using Xunit;

namespace JobSityChat.Infrastructure.Tests
{
    public class CommandHandlerTests
    {
        [Theory]
        [InlineData("/", false)]
        [InlineData("life_is_beautiful", false)]
        [InlineData("<", false)]
        [InlineData("^", false)]
        [InlineData("/yes_it_is", true)]
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
        [InlineData("try_to_be_the_best_of_you", false)]
        [InlineData("/that's the way to go", true)]
        [InlineData("//", true)]
        [InlineData("/price=", true)]
        [InlineData("/shoes=nike", true)]
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
