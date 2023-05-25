using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using ABMVantage.Data.Service;
using ABMVantage_Outbound_UnitTests.Utility.ParamBuilder;
using ABMVantage_Outbound_UnitTests.Utility.TestDataBuilder;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ABMVantage_Outbound_UnitTests.ReservationTests
{
    public class GetMonthlyReservationsTests
    {
        private ITransaction_NewService? _transactionService;
        private readonly Mock<ILoggerFactory> _mockLogger;
        private readonly Mock<IRepository> _mockRepository;
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;

        public GetMonthlyReservationsTests()
        {
            _mockLogger = new Mock<ILoggerFactory>();
            _mockRepository = new Mock<IRepository>();
            _mockTransactionRepository = new Mock<ITransactionRepository> { CallBase = true };
        }

        [Fact]
        public async Task GetMonthlyReservationsTest_Sucess()
        {
            var mockLogger = new Mock<ILogger<Transaction_NewService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            FilterParam filterParam = ParameterBuilder.GetFilterParams();

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);

            _mockTransactionRepository.Setup(x => x.GetRevenueVsBudget(filterParam)).ReturnsAsync(TestDataBuilder.GetRevenueBudgetTestData());
            _mockRepository.SetupGet(x => x.TransactionRepository).Returns(_mockTransactionRepository.Object);

            _transactionService = new Transaction_NewService(_mockLogger.Object, _mockRepository.Object, null, null);

            List<RevenueBudget> result = new List<RevenueBudget>(await _transactionService.GetRevenueVsBudget(filterParam));

            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task GetMonthlyReservationsTest_Failure()
        {
            var mockLogger = new Mock<ILogger<Transaction_NewService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            FilterParam filterParam = ParameterBuilder.GetFilterParams();

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);

            _mockTransactionRepository.Setup(x => x.GetRevenueVsBudget(new FilterParam())).ReturnsAsync(TestDataBuilder.GetRevenueBudgetTestData());
            _mockRepository.SetupGet(x => x.TransactionRepository).Returns(_mockTransactionRepository.Object);

            _transactionService = new Transaction_NewService(_mockLogger.Object, _mockRepository.Object, null, null);

            List<RevenueBudget> result = new List<RevenueBudget>(await _transactionService.GetRevenueVsBudget(filterParam));

            Assert.Empty(result);
        }
    }
}