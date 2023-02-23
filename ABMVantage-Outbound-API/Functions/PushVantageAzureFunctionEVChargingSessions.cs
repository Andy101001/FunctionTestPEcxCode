using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ABMVantage_Outbound_API.Functions
{
    public class PushVantageAzureFunctionEVChargingSessions
    {
        private readonly ILogger _logger;
        private readonly IActiveClosedEvChargingSessions _chargingSessions;

        public PushVantageAzureFunctionEVChargingSessions(ILoggerFactory loggerFactory, IActiveClosedEvChargingSessions chargingSessions)
        {
            ArgumentNullException.ThrowIfNull(chargingSessions);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionReservations>();
            _chargingSessions = chargingSessions;
        }

        [Function("PushVantageAzureFunctionEVChargingSessions")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "chargingSessions")] HttpRequestData req)
        {
            _logger.LogInformation("Executing function Get Reservatons");
            var result = await _chargingSessions.GetChargingSessionsAsync();
            _logger.LogInformation("Executed Function Get Reservatons");

            return new OkObjectResult(result);
        }

    }
}