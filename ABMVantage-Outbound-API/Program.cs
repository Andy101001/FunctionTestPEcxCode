using ABMVantage_Outbound_API.Configuration;
using ABMVantage_Outbound_API.DataAccess;
using ABMVantage_Outbound_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ABMVantage_Outbound_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IConfiguration? Configuration { get; set; }

        /// <summary>
        /// Create the host builder for the application
        /// </summary>
        /// <param name="args">Argument strings</param>
        /// <returns>A <see cref="IHostBuilder"/></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
            ConfigureFunctionsWorkerDefaults(builder =>
            {
                builder.UseDefaultWorkerMiddleware();
            }).ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                // load environment variables into the config
                configurationBuilder.AddEnvironmentVariables();

                // Get the configuration
                Configuration = context.Configuration;



                // load Azure App Configuration variables
                //configurationBuilder.AddAppConfiguration();

#if DEBUG
                // finally, ensure that appsettings.Development.json overrides all in debug mode
                configurationBuilder.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

#endif
            }).ConfigureServices(s =>
            {
                s.AddScoped<IObsReservationService, ObsReservationService>();
                s.AddScoped<IDataAccessService, DataAccessService>();
                s.AddScoped<IActiveClosedEvChargingService, ActiveClosedEvChargingService>();
                s.AddScoped<IOBSReservationTransactionsService, OBSReservationTransactionsService>();
                s.AddScoped<ITicketOccupanciesService, TicketOccupanciesService>();
                s.AddScoped<IPgsTicketOccupanciesService, PgsTicketOccupanciesService>();
                s.AddScoped<IParcsTicketTransactionsService, ParcsTicketTransactionsService>();

                s.AddOptions();

                s.AddDbContextFactory<CosmosDataContext>(
                (IServiceProvider sp, DbContextOptionsBuilder opts) =>
                {
                    var cosmosSettings = sp
                        .GetRequiredService<IOptions<CosmosSettings>>()
                        .Value;

                    opts.UseCosmos(
                        cosmosSettings.EndPoint = "https://abm-vtg-cosmos01-dev.documents.azure.com:443/",
                        cosmosSettings.AccessKey = "6CYkip2ZaFNGEsWy1JXWFe4LuV1fAJOOVDHeooIjmOWnxizz9BbWAkah3MEnjb8014upO3D91wuuACDb4rR0xg==",
                        databaseName: "VantageDB");
                });
            });
    }
}
