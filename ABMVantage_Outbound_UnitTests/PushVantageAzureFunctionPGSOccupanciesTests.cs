namespace ABMVantage_Outbound_UnitTests
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;


    public class PushVantageAzureFunctionPGSOccupanciesTests
    {
        private IPgsTicketOccupanciesService? _pgsTicketOccupanciesService;
        private readonly Mock<ILoggerFactory> _mockLogger;
        private readonly Mock<IDataAccessService> _mockDataAccessService;

        /// <summary>
        /// ctor
        /// </summary>        
        public PushVantageAzureFunctionPGSOccupanciesTests()
        {
            _mockLogger = new Mock<ILoggerFactory>();
            _mockDataAccessService = new Mock<IDataAccessService>();

        }

        [Fact]
        public async Task PushVantageAzureFunctionPGSOccupanciesTests_Success()
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

            _mockDataAccessService.Setup(x => x.GetPgsTicketOccupanciesAsync()).ReturnsAsync(new List<PgsOccupancy> 
            {
                new PgsOccupancy { Id = "testID" } 
            });


            _pgsTicketOccupanciesService = new PgsTicketOccupanciesService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _pgsTicketOccupanciesService.GetOccupanciesAsync();

            Assert.NotNull(result);
            
        }
        [Fact]
        public async Task PushVantageAzureFunctionPGSOccupanciesTests_Failure()
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

            _mockDataAccessService.Setup(x => x.GetPgsTicketOccupanciesAsync()).ReturnsAsync(new List<PgsOccupancy>());


            _pgsTicketOccupanciesService = new PgsTicketOccupanciesService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _pgsTicketOccupanciesService.GetOccupanciesAsync();

            Assert.Empty(result);
        }
    }
}