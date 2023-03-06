﻿namespace ABMVantage_Outbound_UnitTests
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;


    public class PushVantageAzureFunctionPARCSTicketsTransactionsTests
    {
        private IParcsTicketTransactionsService? _parcsTicketTransactionsService;
        private readonly Mock<ILoggerFactory> _mockLogger;
        private readonly Mock<IDataAccessService> _mockDataAccessService;

        /// <summary>
        /// ctor
        /// </summary>        
        public PushVantageAzureFunctionPARCSTicketsTransactionsTests()
        {
            _mockLogger = new Mock<ILoggerFactory>();
            _mockDataAccessService = new Mock<IDataAccessService>();

        }

        [Fact]
        public async Task PushVantageAzureFunctionPARCSTicketsTransactionsTests_Success()
        {

            var mockLogger = new Mock<ILogger<TicketOccupanciesService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);

            _mockDataAccessService.Setup(x => x.GetParcsTicketTransactionsAsync()).ReturnsAsync(new List<ParcsTicketsTransactions> 
            {
                new ParcsTicketsTransactions { Id = "testID" } 
            });


            _parcsTicketTransactionsService = new ParcsTicketTransactionsService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _parcsTicketTransactionsService.GetParcsTicketTransactionsAsync();

            Assert.NotNull(result);
            
        }
        [Fact]
        public async Task PushVantageAzureFunctionPARCSTicketsTransactionsTests_Failure()
        {
            var mockLogger = new Mock<ILogger<TicketOccupanciesService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);

            _mockDataAccessService.Setup(x => x.GetParcsTicketTransactionsAsync()).ReturnsAsync(new List<ParcsTicketsTransactions>());


            _parcsTicketTransactionsService = new ParcsTicketTransactionsService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _parcsTicketTransactionsService.GetParcsTicketTransactionsAsync();

            Assert.Empty(result);
        }
    }
}