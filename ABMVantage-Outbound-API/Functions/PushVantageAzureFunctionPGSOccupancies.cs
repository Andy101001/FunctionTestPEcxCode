namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Gets the pgs tickets occupancy for online book reservations
    /// </summary>
    public class PushVantageAzureFunctionPGSOccupancies
    {
        private readonly ILogger _logger;
        private readonly IPgsTicketOccupanciesService _ticketOccupanciesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushVantageAzureFunctionPGSOccupancies"/> class.
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="reservationTransactionsService">pgs tickets occupancies Service</param>
        public PushVantageAzureFunctionPGSOccupancies(ILoggerFactory loggerFactory, IPgsTicketOccupanciesService ticketOccupanciesService)
        {
            ArgumentNullException.ThrowIfNull(ticketOccupanciesService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionPGSOccupancies>();
            _ticketOccupanciesService = ticketOccupanciesService;
            _logger.LogInformation($"Constructing {nameof(PushVantageAzureFunctionPGSOccupancies)}");
        }

        /// <summary>
        /// Function to push pgs ticket occupancies transactions.
        /// </summary>
        /// <param name="req">request data</param>
        /// <returns>IActionResult</returns>
        [Function("PushVantageAzureFunctionPGSOccupancies")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/pgs/occupancies")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(PushVantageAzureFunctionPGSOccupancies)}");
            var result = await _ticketOccupanciesService.GetOccupanciesAsync();
            _logger.LogInformation($"Executed function {nameof(PushVantageAzureFunctionPGSOccupancies)}");

            return new OkObjectResult(result);
        }
    }
}