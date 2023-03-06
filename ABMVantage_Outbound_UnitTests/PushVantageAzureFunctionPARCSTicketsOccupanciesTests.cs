namespace ABMVantage_Outbound_UnitTests
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;


    public class PushVantageAzureFunctionPARCSTicketsOccupanciesTests
    {
        private ITicketOccupanciesService? _ticketOccupanciesService;
        private readonly Mock<ILoggerFactory> _mockLogger;
        private readonly Mock<IDataAccessService> _mockDataAccessService;

        /// <summary>
        /// ctor
        /// </summary>        
        public PushVantageAzureFunctionPARCSTicketsOccupanciesTests()
        {
            _mockLogger = new Mock<ILoggerFactory>();
            _mockDataAccessService = new Mock<IDataAccessService>();

        }

        [Fact]
        public async Task PushVantageAzureFunctionPARCSTicketsOccupanciesTests_Success()
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

            _mockDataAccessService.Setup(x => x.GetParcsTicketOccupanciesAsync()).ReturnsAsync(new List<Occupancy> 
            {
                new Occupancy { Id = "testID" } 
            });


            _ticketOccupanciesService = new TicketOccupanciesService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _ticketOccupanciesService.GetOccupanciesAsync();

            Assert.NotNull(result);
            
        }
        [Fact]
        public async Task PushVantageAzureFunctionPARCSTicketsOccupanciesTests_Failure()
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

            _mockDataAccessService.Setup(x => x.GetParcsTicketOccupanciesAsync()).ReturnsAsync(new List<Occupancy>());


            _ticketOccupanciesService = new TicketOccupanciesService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _ticketOccupanciesService.GetOccupanciesAsync();

            Assert.Empty(result);
        }
    }
}