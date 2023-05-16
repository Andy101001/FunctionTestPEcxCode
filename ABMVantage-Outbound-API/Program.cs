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
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.OpenApi.Models;
using AutoFixture;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Tools;
using ABMVantage.Data.Repository;
using System.Configuration;
using ABMVantage.Data.Service;
using StackExchange.Redis;
using ABMVantage.Data.Configuration;
using ABMVantage.Data.DataAccess;

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

                //Read Redis settings from configuration or local settings
                builder.Services.AddOptions<RedisSettings>().Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("RedisSettings").Bind(settings);
                });

                //Read Sql settings from configuration or local settings
                builder.Services.AddOptions<SqlSettings>().Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("SqlSettings").Bind(settings);
                });

                //Read Dashboard function settings from configuration or local settings
                builder.Services.AddOptions<DashboardFunctionSettings>().Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("DashboardFunctionSettings").Bind(settings);
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
                s.AddScoped<IDataAccessSqlService, DataAccessSqlService>();
                s.AddScoped<IDataAccessService, DataAccessService>();
                s.AddScoped<ITransactionService, TransactionService>();
                s.AddScoped<IParkingOccupancyService, ParkingOccupancyService>();
                s.AddScoped<IFloorDetailsService, FloorDetailsService>();
                s.AddScoped<ITransactionService, TransactionService>();
                s.AddScoped<IRevenueService, RevenueService>();
                //Dapper
                s.AddScoped<IDapperConnection>(d => new DapperConnection(s.BuildServiceProvider().GetRequiredService<IOptions<SqlSettings>>().Value.ConnectionString));
                s.AddScoped<IRepository, Repository>();

                //Data Services
                s.AddScoped<IOccupancyService, OccupancyService>();
                s.AddScoped<IReservationNTicketService, ReservationNTicketService>();
                s.AddScoped<IFilterDataService, FilterDataService>();
                s.AddScoped<IRepository, Repository>();

                s.AddScoped<IReservationService, ReservationService>();
                s.AddScoped<ITicketService, TicketService>();

                s.AddScoped<ITransaction_NewService, Transaction_NewService>();
                s.AddScoped<IRedisCachingService, RedisCachingService>();

                //Redis Cache Configuration
                var redisSettings = s.BuildServiceProvider().GetRequiredService<IOptions<RedisSettings>>().Value;
                //string redisConnection = Configuration["RedisCacheOptions:Configuration"]; ;
                var multiplexer = ConnectionMultiplexer.Connect(redisSettings.ConntecitonString);
                s.AddSingleton<IConnectionMultiplexer>(multiplexer);
                s.AddScoped<IDataCosmosAccessService, DataCosmosAccessService>();

                

                s.AddOptions();

                s.AddSingleton<Fixture>(new Fixture())
                .AddSingleton<IOpenApiConfigurationOptions>(_ =>
                {
                    var options = new OpenApiConfigurationOptions()
                    {
                        Info = new OpenApiInfo()
                        {
                            Version = DefaultOpenApiConfigurationOptions.GetOpenApiDocVersion(),
                            Title = $"{DefaultOpenApiConfigurationOptions.GetOpenApiDocTitle()} (Injected)",
                            Description = DefaultOpenApiConfigurationOptions.GetOpenApiDocDescription(),
                            TermsOfService = new Uri("https://github.com/Azure/azure-functions-openapi-extension"),
                            Contact = new OpenApiContact()
                            {
                                Name = "Enquiry",
                                Email = "azfunc-openapi@microsoft.com",
                                Url = new Uri("https://github.com/Azure/azure-functions-openapi-extension/issues"),
                            },
                            License = new OpenApiLicense()
                            {
                                Name = "MIT",
                                Url = new Uri("http://opensource.org/licenses/MIT"),
                            }
                        },
                        Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                        OpenApiVersion = DefaultOpenApiConfigurationOptions.GetOpenApiVersion(),
                        IncludeRequestingHostName = DefaultOpenApiConfigurationOptions.IsFunctionsRuntimeEnvironmentDevelopment(),
                        ForceHttps = DefaultOpenApiConfigurationOptions.IsHttpsForced(),
                        ForceHttp = DefaultOpenApiConfigurationOptions.IsHttpForced(),
                    };

                    return options;
                }).AddSingleton<IOpenApiHttpTriggerAuthorization>(_ =>
                {
                    var auth = new OpenApiHttpTriggerAuthorization(req =>
                    {
                        var result = default(OpenApiAuthorizationResult);

                        // ⬇️⬇️⬇️ Add your custom authorisation logic ⬇️⬇️⬇️
                        //
                        // CUSTOM AUTHORISATION LOGIC
                        //
                        // ⬆️⬆️⬆️ Add your custom authorisation logic ⬆️⬆️⬆️

                        return Task.FromResult(result);
                    });

                    return auth;
                }).AddSingleton<IOpenApiCustomUIOptions>(_ =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var options = new OpenApiCustomUIOptions(assembly)
                    {
                        GetStylesheet = () =>
                        {
                            var result = string.Empty;

                            // ⬇️⬇️⬇️ Add your logic to get your custom stylesheet ⬇️⬇️⬇️
                            //
                            // CUSTOM LOGIC TO GET STYLESHEET
                            //
                            // ⬆️⬆️⬆️ Add your logic to get your custom stylesheet ⬆️⬆️⬆️

                            return Task.FromResult(result);
                        },
                        GetJavaScript = () =>
                        {
                            var result = string.Empty;

                            // ⬇️⬇️⬇️ Add your logic to get your custom JavaScript ⬇️⬇️⬇️
                            //
                            // CUSTOM LOGIC TO GET JAVASCRIPT
                            //
                            // ⬆️⬆️⬆️ Add your logic to get your custom JavaScript ⬆️⬆️⬆️

                            return Task.FromResult(result);
                        }
                    };

                    return options;
                });

                // Read the settings from the function config or the local settings file during debug
                var securitySettings = s.BuildServiceProvider().GetRequiredService<IOptions<SecuritySettings>>().Value;
                var cosmosSettings = s.BuildServiceProvider().GetRequiredService<IOptions<CosmosSettings>>().Value;
                var sqlSettings = s.BuildServiceProvider().GetRequiredService<IOptions<SqlSettings>>().Value;
                var dashboardSettings = s.BuildServiceProvider().GetRequiredService<IOptions<DashboardFunctionSettings>>().Value;


                if (dashboardSettings != null)
                {
                    s.AddScoped<DashboardFunctionSettings, DashboardFunctionSettings>(sp =>
                        dashboardSettings
                    );
                }

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

                //// Add the cosmos db context factory
                s.AddDbContextFactory<SqlDataContext>((IServiceProvider sp, DbContextOptionsBuilder opts) =>
                {
                    // Make sure we successfully read in the security settings
                    if (sqlSettings != null)
                    {
                        // Check for the null strings
                        if (!string.IsNullOrEmpty(sqlSettings.ConnectionString))
                        {
                            opts.UseSqlServer(sqlSettings.ConnectionString);
                        }
                    }
                });



            });
    }
}
