namespace ABMVantage_Outbound_UnitTests
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;


    public class PushVantageAzureFunctionReservationsTests
    {
        private IObsReservationService? _obsReservationService;
        private readonly Mock<ILoggerFactory> _mockLogger;
        private readonly Mock<IDataAccessService> _mockDataAccessService;

        /// <summary>
        /// ctor
        /// </summary>        
        public PushVantageAzureFunctionReservationsTests()
        {
            _mockLogger = new Mock<ILoggerFactory>();
            _mockDataAccessService = new Mock<IDataAccessService>();

        }

        [Fact]
        public async Task PushVantageAzureFunctionReservationsTests_Success()
        {

            var mockLogger = new Mock<ILogger<ObsReservationService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);

            _mockDataAccessService.Setup(x => x.GetAllObsReservationsAsync()).ReturnsAsync(new List<Booking> 
            {
                new Booking { Id = "testID" } 
            });


            _obsReservationService = new ObsReservationService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _obsReservationService.GetAllReservationsAsync();

            Assert.NotNull(result);
            
        }
        [Fact]
        public async Task PushVantageAzureFunctionReservationsTests_Failure()
        {
            var mockLogger = new Mock<ILogger<ObsReservationService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);
            _mockDataAccessService.Setup(x => x.GetAllObsReservationsAsync()).ReturnsAsync(new List<Booking>());


            _obsReservationService = new ObsReservationService(_mockLogger.Object, _mockDataAccessService.Object);

            var result = await _obsReservationService.GetAllReservationsAsync();

            Assert.False(result.Any());
        }
    }
}