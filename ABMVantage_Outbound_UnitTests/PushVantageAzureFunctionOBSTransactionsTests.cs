namespace ABMVantage_Outbound_UnitTests
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Models;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;


    public class PushVantageAzureFunctionOBSTransactionsTests
    {
        private IOBSReservationTransactionsService? _obsReservationTransactionsService;
        private readonly Mock<ILoggerFactory> _mockLogger;
        private readonly Mock<IDataAccessService> _mockDataAccessService;

        public PushVantageAzureFunctionOBSTransactionsTests()
        {
            _mockLogger = new Mock<ILoggerFactory>();
            _mockDataAccessService = new Mock<IDataAccessService>();

        }

        [Fact]
        public async Task PushVantageAzureFunctionOBSTransactionsTests_Success()
        {

            var mockLogger = new Mock<ILogger<OBSReservationTransactionsService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);

            _mockDataAccessService.Setup(x => x.GetReservationsTransactionsAsync()).ReturnsAsync(new List<ObsReservationTransactions> 
            {
                new ObsReservationTransactions {id = "testId"} 
            });
            _obsReservationTransactionsService = new OBSReservationTransactionsService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _obsReservationTransactionsService.GetObsReservationTransactionsAsync();

            Assert.NotNull(result);
            
        }
        [Fact]
        public async Task PushVantageAzureFunctionOBSTransactionsTests_Failure()
        {
            var mockLogger = new Mock<ILogger<OBSReservationTransactionsService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);

            _mockDataAccessService.Setup(x => x.GetReservationsTransactionsAsync()).ReturnsAsync(new List<ObsReservationTransactions>());
            _obsReservationTransactionsService = new OBSReservationTransactionsService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _obsReservationTransactionsService.GetObsReservationTransactionsAsync();

            Assert.NotNull(result);
        }
    }
}