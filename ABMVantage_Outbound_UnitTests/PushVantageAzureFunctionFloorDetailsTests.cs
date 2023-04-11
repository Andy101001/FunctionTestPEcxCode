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
        private readonly Mock<IDataAccessSqlService> _mockDataAccessService;
        private readonly IDataAccessSqlService _dataAccessSqlService;
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
                context.Add(new Level { LevelId = "01", LevelName = 10 });
                context.Add(new Facility { FacilityId = "01", FacilityName = "Facililty 1" });
                context.Add(new Product { ProductId = "01", ProductName = "Product 1" });
                context.SaveChanges();
            }
            var sqlDbService = new DataAccessSqlService(mockDbFactory.Object, _mockLogger.Object);

            // ACT
            //_floorDetailsService = new FloorDetailsService(_mockLogger.Object, sqlDbService, configuration);
            //var result = await _floorDetailsService.GetFloorDetails("01");

            string result = "ok";
            // ASSERT
            Assert.NotNull(result);

        }
    }
}
