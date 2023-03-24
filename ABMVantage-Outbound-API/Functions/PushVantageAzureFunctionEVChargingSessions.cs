namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Models;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
    using Microsoft.Extensions.Logging;
    using System.Net;

    /// <summary>
    /// class to push ev charging sessions
    /// </summary>
    public class PushVantageAzureFunctionEVChargingSessions
    {
        private readonly ILogger _logger;
        private readonly IActiveClosedEvChargingService _chargingSessions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushVantageAzureFunctionEVChargingSessions"/> class.
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="chargingSessions"></param>
        public PushVantageAzureFunctionEVChargingSessions(ILoggerFactory loggerFactory, IActiveClosedEvChargingService chargingSessions)
        {
            ArgumentNullException.ThrowIfNull(chargingSessions);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionEVChargingSessions>();
            _chargingSessions = chargingSessions;
            _logger.LogInformation($"Constructing {nameof(PushVantageAzureFunctionEVChargingSessions)}");
        }

        /// <summary>
        /// Function to push ev charging sessions
        /// </summary>
        /// <param name="req">req data</param>
        /// <returns>IActionResult</returns>
        [Function("PushVantageAzureFunctionEVChargingSessions")]
        [OpenApiOperation(operationId: "PushVantageAzureFunctionEVChargingSessions", tags: new[] { "PushVantageAzureFunctionEVChargingSessions" }, Summary = "Get Charging Sessions", Description = "This gets an existing charging session.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ActiveClosedEvChargingSession), Summary = "Get Charging Sessions", Description = "Get Charging Sessions")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Charging Session not found", Description = "Charging Session not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/vantagePortal/ev/chargingSessions")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(PushVantageAzureFunctionEVChargingSessions)}");
            var result = await _chargingSessions.GetChargingSessionsAsync();
            _logger.LogInformation($"Executed function {nameof(PushVantageAzureFunctionEVChargingSessions)}");

            return new OkObjectResult(result);
        }
    }
}