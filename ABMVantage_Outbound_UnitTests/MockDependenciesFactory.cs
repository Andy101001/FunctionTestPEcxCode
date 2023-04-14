using ABMVantage_Outbound_API.DataAccess;
using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_UnitTests
{
    public class MockDependenciesFactory
    {
        internal static DataAccessSqlService CreateMockDataAccessService(IEnumerable<object> dataEntities)
        {
            var mockDbFactory = new Mock<IDbContextFactory<SqlDataContext>>();
            using (var context = mockDbFactory.Object.CreateDbContext())
            {
                foreach (var entity in dataEntities)
                {
                    context.Add(entity);
                }
                context.SaveChanges();
            }
            var mockLoggerFactory = CreateMockLoggerFactory<DataAccessSqlService>();
            var dataAccessService = new DataAccessSqlService(mockDbFactory.Object, mockLoggerFactory);
            return dataAccessService;
        }

        internal static ILoggerFactory CreateMockLoggerFactory<T>()
        {
            var mockLogger = new Mock<ILogger<T>>();
            mockLogger.Setup(
                           m => m.Log(
                               LogLevel.Information,
                               It.IsAny<EventId>(),
                               It.IsAny<object>(),
                               It.IsAny<Exception>(),
                               It.IsAny<Func<object, Exception, string>>()));
            
            var mockLoggerFactory = new Mock<Microsoft.Extensions.Logging.ILoggerFactory>();
            mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);
            return mockLoggerFactory.Object;
        }

        public static IEnumerable<object> GetTestParkingSpaceData()
        {
            var parkingSpaceData = new object[]
            {
                new DimCustomer { CustomerBuKey = 01, BuCode = "LAX", CustomerId = "PQR_1576" },
                new DimLocation { BuCode = "LAX", LocationId = 3576 },
                new DimFacility { FacilityId = "LAX3576BLDG01", FacilityName = "LAXPARKINGBLDG01", LocationId = 3576 },
                new DimLevel { LevelId = "01", LavelName = 5, FacilityId = "LAX3576BLDG01" },
                new DimParkingSpace { ParkingProductId = "2545", ParkingSpaceId = "LAX_P1-L1-S3" },
                new SpaceProduct { ParkingSpaceId = "LAX_P1-L1-S3", ParkingProductId = 2545 },
                new DimProduct { ProductId = 2545, ProductName = "Product 1" }
            };
            return parkingSpaceData;
        }
    }
}
