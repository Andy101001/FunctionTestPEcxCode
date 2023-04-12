namespace ABMVantage_Outbound_UnitTests
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;


    public class PushVantageAzureFunctionEVChargingSessionsTests
    {
        private IActiveClosedEvChargingService? _activeClosedEvChargingService;
        private readonly Mock<ILoggerFactory> _mockLogger;
        private readonly Mock<IDataAccessService> _mockDataAccessService;

        /// <summary>
        /// 
        /// </summary>
        public PushVantageAzureFunctionEVChargingSessionsTests()
        {
            _mockLogger = new Mock<ILoggerFactory>();
            _mockDataAccessService = new Mock<IDataAccessService>();

        }

        [Fact]
        public async Task PushVantageAzureFunctionEVChargingSessionsTests_Success()
        {

            var mockLogger = new Mock<ILogger<ActiveClosedEvChargingService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);

            _mockDataAccessService.Setup(x => x.GetActiveChargingSessionsAsync("testId")).ReturnsAsync(new List<EvActiveSessions> 
            {
                new EvActiveSessions { Id = "testID" } 
            });

            _mockDataAccessService.Setup(x => x.GetClosedChargingSessionsAsync("testId")).ReturnsAsync(new List<EvClosedSessions>
            {
                new EvClosedSessions { Id = "testID" }
            });


            _activeClosedEvChargingService = new ActiveClosedEvChargingService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _activeClosedEvChargingService.GetChargingSessionsAsync();

            Assert.NotNull(result);
            
        }
        [Fact]
        public async Task PushVantageAzureFunctionEVChargingSessionsTests_Failure()
        {
            var mockLogger = new Mock<ILogger<ActiveClosedEvChargingService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);


            _mockDataAccessService.Setup(x => x.GetActiveChargingSessionsAsync("")).ReturnsAsync(new List<EvActiveSessions>());
            _mockDataAccessService.Setup(x => x.GetClosedChargingSessionsAsync("testId")).ReturnsAsync(new List<EvClosedSessions>());

            _activeClosedEvChargingService = new ActiveClosedEvChargingService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _activeClosedEvChargingService.GetChargingSessionsAsync();

            Assert.Null(result.ClosedSessions);
            Assert.Null(result.ActiveSessions);
        }
    }
}