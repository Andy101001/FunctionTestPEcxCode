namespace ABMVantage_Outbound_UnitTests
{
    using ABMVantage_Outbound_API.DataAccess;
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System.Configuration;
    using System.Data.Common;
    using System.Text;
    using Xunit;

    public class PushVantageAzureFunctionFloorDetailsTests
    {
        private IFloorDetailsService? _floorDetailsService;
        private readonly Mock<ILoggerFactory> _mockLogger;
        private readonly IDataAccessService _dataAccessService;
        private readonly IDataAccessSqlService _dataAccessSqlService;
        private IConfiguration configuration;
        public PushVantageAzureFunctionFloorDetailsTests()
        {
            _mockLogger = new Mock<ILoggerFactory>();
         
        }

        [Fact]
        public async Task PushVantageAzureFunctionFloorDetailsTestS_Success()
        {
            // ARRANGE

            var appSettings = @"{""SqlSettings"":{
            ""IsSqlDbConnectionOn"" : ""true""
            }}";
            var builder = new ConfigurationBuilder();
            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));
            var configuration = builder.Build();


            var mockDbFactory = new Mock<IDbContextFactory<SqlDataContext>>();

            mockDbFactory.Setup(f => f.CreateDbContext()).Returns(() => new SqlDataContext(new DbContextOptionsBuilder<SqlDataContext>()
                    .UseInMemoryDatabase("InMemoryTest")
                    .Options));

            var mockLogger = new Mock<ILogger<IFloorDetailsService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);


            var mockdbLogger = new Mock<ILogger<DataAccessSqlService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            _mockLogger.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);

            using (var context = mockDbFactory.Object.CreateDbContext())
            {
                context.Add(new DimCustomer {CustomerBuKey=01,BuCode= "LAX", CustomerId= "PQR_1576" });
                context.Add(new DimLocation {BuCode= "LAX", LocationId= 3576 });
                context.Add(new DimFacility { FacilityId = "LAX3576BLDG01", FacilityName = "LAXPARKINGBLDG01", LocationId= 3576 });
                context.Add(new DimLevel { LevelId = "01",LavelName=5, FacilityId= "LAX3576BLDG01" });
                context.Add(new DimParkingSpace {ParkingProductId= "2545", ParkingSpaceId= "LAX_P1-L1-S3" });
                context.Add(new SpaceProduct {ParkingSpaceId = "LAX_P1-L1-S3",ParkingProductId= 2545 });
                context.Add(new DimProduct { ProductId = 2545, ProductName = "Product 1" });
                context.SaveChanges();
            }
            var sqlDbService = new DataAccessSqlService(mockDbFactory.Object, _mockLogger.Object);

            // ACT
            _floorDetailsService = new FloorDetailsService(_mockLogger.Object, sqlDbService, _dataAccessService, configuration);
            var result = await _floorDetailsService.GetFloorDetails("PQR_1576");

            // ASSERT
            Assert.NotNull(result);

        }
    }
}
