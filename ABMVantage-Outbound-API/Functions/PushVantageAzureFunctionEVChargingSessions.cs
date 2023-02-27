namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;

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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/vantagePortal/ev/chargingSessions")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(PushVantageAzureFunctionEVChargingSessions)}");
            var result = await _chargingSessions.GetChargingSessionsAsync();
            _logger.LogInformation($"Executed function {nameof(PushVantageAzureFunctionEVChargingSessions)}");

            return new OkObjectResult(result);
        }
    }
}