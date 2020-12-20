using System;
using System.Threading.Tasks;
using JobSityChat.Core.Entities;
using JobSityChat.Infrastructure.Services.Handlers;
using Xunit;

namespace JobSityChat.Infrastructure.Tests
{
    public class StockHandlerTest
    {
        [Theory]
        [InlineData("warrior", false)]
        [InlineData("be_brave", false)]
        [InlineData("aapl.us", true)]
        public async Task GetStockData_ShouldReturnTheStockObjectIfTheStockApiReturn200
            (string url, bool shouldReturnStock)
        {
            StockHandler stockHandler = new StockHandler();

            //Arrange
            
            //Act
            Stock stock = await stockHandler.GetStockData(url);

            //Assert
            if (shouldReturnStock)
            {
                Assert.True(stock != null);
            }else
            {
                Assert.True(stock == null);
            }
        }
    }
}
