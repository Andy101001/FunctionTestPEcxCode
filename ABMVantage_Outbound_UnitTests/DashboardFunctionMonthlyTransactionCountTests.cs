using ABMVantage_Outbound_API.Configuration;
using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ABMVantage_Outbound_UnitTests
{
    public class DashboardFunctionMonthlyTransactionCountTests
    {
        //TODO: Write a better test after we choose a better ORM framework
        /*
        [Fact]
        public async Task DashboardFunctionMonthlyTransactionCountTests_Success()
        {
            // Arrange
            var mockLoggerFactory = MockDependenciesFactory.CreateMockLoggerFactory<TransactionService>();
            IEnumerable<object> testData = GetTestData();
            //var mockDataAccessService = MockDependenciesFactory.CreateMockDataAccessService(testData);
            var mockSettings = new DashboardFunctionSettings{ MonthlyTransactionCountInterval = 3, MinimumValidCalculationDate = DateTime.Parse("1900-01-01")  };
            

            // Act
            var testTarget = new TransactionService(mockLoggerFactory, mockDataAccessService, mockSettings);
            var calculationDate = DateTime.Parse("2021-01-01");
            var facilityId = "testFacilityId";
            var levelId = "testLevelId";    
            var parkingProductId = "testParkingProductId";
            var result = await testTarget.GetMonthlyTransactionCount(calculationDate, facilityId, levelId, parkingProductId);
            
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.MonthlyTransactions);
            Assert.Equal(2, result.MonthlyTransactions.Count());
        }

        private IEnumerable<object> GetTestData()
        {
            List<object> testData = new List<object>();
            testData.AddRange(MockDependenciesFactory.GetTestParkingSpaceData());
            testData.Add(new FactPaymentsTicketsAndStaged { PaymentId = "TestPaymentId1", TicketId = "TestTicketId1", PaymentDateTimeUtc = DateTime.Parse("2020-06-01")});
            testData.Add(new FactPaymentsTicketsAndStaged { PaymentId = "TestPaymentId2", TicketId = "TestTicketId2", PaymentDateTimeUtc = DateTime.Parse("2020-07-01") });
            testData.Add(new FactPaymentsTicketsAndStaged { PaymentId = "TestPaymentId3", TicketId = "TestTicketId3", PaymentDateTimeUtc = DateTime.Parse("2021-01-11") });
            testData.Add(new FactPaymentsTicketsAndStaged { PaymentId = "TestPaymentId4", TicketId = "TestTicketId4", PaymentDateTimeUtc = DateTime.Parse("2021-02-07") });
            testData.Add(new FactPaymentsTicketsAndStaged { PaymentId = "TestPaymentId5", TicketId = "TestTicketId5", PaymentDateTimeUtc = DateTime.Parse("2021-02-03") });
            testData.Add(new FactPaymentsTicketsAndStaged { PaymentId = "TestPaymentId6", TicketId = "TestTicketId6", PaymentDateTimeUtc = DateTime.Parse("2021-01-23") });
            testData.Add(new FactTickets { TicketId = "TestTicketId1", ParkingSpaceId = "LAX_P1-L1-S3" });
            testData.Add(new FactTickets { TicketId = "TestTicketId2", ParkingSpaceId = "LAX_P1-L1-S3" });
            testData.Add(new FactTickets { TicketId = "TestTicketId3", ParkingSpaceId = "LAX_P1-L1-S3" });
            testData.Add(new FactTickets { TicketId = "TestTicketId4", ParkingSpaceId = "LAX_P1-L1-S3" });
            testData.Add(new FactTickets { TicketId = "TestTicketId5", ParkingSpaceId = "LAX_P1-L1-S4" });
            testData.Add(new FactTickets { TicketId = "TestTicketId6", ParkingSpaceId = "LAX_P1-L1-S4" });
            return testData;



        }
        */
    }
}
