using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using ABMVantage.Data.Repository;
using ABMVantage.Data.Service;
using ABMVantage_Outbound_API.Services;
using ABMVantage_Outbound_UnitTests.Utility;
using ABMVantage_Outbound_UnitTests.Utility.ParamBuilder;
using ABMVantage_Outbound_UnitTests.Utility.TestDataBuilder;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ABMVantage_Outbound_UnitTests.TransactionTests
{
    public class GetRevenueByMonthsTests
    {
        private ITransaction_NewService? _transactionService;
        private readonly Mock<ILoggerFactory> _mockLogger;        
        private readonly Mock<IRepository> _mockRepository;        
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;

        public GetRevenueByMonthsTests()
        {
            _mockLogger = new Mock<ILoggerFactory>();
            _mockRepository = new Mock<IRepository>();            
            _mockTransactionRepository = new Mock<ITransactionRepository> { CallBase = true };
        }
        
        [Fact]
        public async Task GetRevenueByMonthsTest_Sucess()
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

           

            _transactionService = new Transaction_NewService(_mockRepository.Object);

            List<RevenueBudget> result = new List<RevenueBudget>(await _transactionService.GetRevenueVsBudget(filterParam));
            
            Assert.Equal(4, result.Count);

        }
        
        [Fact]
        public async Task GetRevenueByMonthsTest_Failure()
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



            _transactionService = new Transaction_NewService(_mockRepository.Object);

            List<RevenueBudget> result = new List<RevenueBudget>(await _transactionService.GetRevenueVsBudget(filterParam));

            Assert.Empty(result);

        }

        [Fact]
        public async Task GetBudgetVsActualVariance_Success()
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

            _mockTransactionRepository.Setup(x => x.GetBudgetVsActualVariance(filterParam)).ReturnsAsync(TestDataBuilder.GetBudgetVarianceTestData());
            _mockRepository.SetupGet(x => x.TransactionRepository).Returns(_mockTransactionRepository.Object);

            _transactionService = new Transaction_NewService(_mockRepository.Object);

            List<BudgetVariance> result = new List<BudgetVariance>(await _transactionService.GetBudgetVsActualVariance(filterParam));

            Assert.Equal(4, result.Count);
        }
    }
}