using ABMVantage_Outbound_API.Configuration;
using ABMVantage_Outbound_API.DataAccess;
using ABMVantage_Outbound_API.Services;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;

namespace ABMVantage_Outbound_API
{
    /// <summary>
    /// Start up for isolated functions
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Static CTOR
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// IConfiguration
        /// </summary>
        public static IConfiguration? Configuration { get; set; }

        /// <summary>
        /// Configuration Builder
        /// </summary>
        public static IConfigurationBuilder? ConfigurationBuilder { get; set; }

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
                //Read security settings from configuration or local settings
                builder.Services.AddOptions<SecuritySettings>().Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("SecurityOptions").Bind(settings);
                });

                //Read cosmos settings from configuration or local settings
                builder.Services.AddOptions<CosmosSettings>().Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("CosmosSettings").Bind(settings);
                });

            }).ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                // load environment variables into the config
                configurationBuilder.AddEnvironmentVariables();

                // Get the configurations we need to add kv and app configs
                Configuration = context.Configuration;
                ConfigurationBuilder = configurationBuilder;

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

                // Read the settings from the function config or the local settings file during debug
                var securitySettings = s.BuildServiceProvider().GetRequiredService<IOptions<SecuritySettings>>().Value;
                var cosmosSettings = s.BuildServiceProvider().GetRequiredService<IOptions<CosmosSettings>>().Value;


                // Make sure we successfully read in the security settings
                if (securitySettings != null)
                {
                    // Check for the null strings
                    if (!string.IsNullOrEmpty(securitySettings.KeyVaultUri))
                        ConfigurationBuilder.AddAzureKeyVault(new Uri(securitySettings.KeyVaultUri), new DefaultAzureCredential());
                }

                //// Add the cosmos db context factory
                s.AddDbContextFactory<CosmosDataContext>((IServiceProvider sp, DbContextOptionsBuilder opts) =>
                {
                    // Make sure we successfully read in the security settings
                    if (cosmosSettings != null)
                    {
                        // Check for the null strings
                        if (!string.IsNullOrEmpty(cosmosSettings.EndPoint) &&
                            !string.IsNullOrEmpty(cosmosSettings.AccessKey) &&
                            !string.IsNullOrEmpty(cosmosSettings.DatabaseName))
                        {
                            opts.UseCosmos(cosmosSettings.EndPoint, cosmosSettings.AccessKey, databaseName: cosmosSettings.DatabaseName);
                        }
                    }
                });
            });
    }
}
