namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Gets the transactinos for online book reservations
    /// </summary>
    public class PushVantageAzureFunctionPARCSTicketsOccupancies
    {
        private readonly ILogger _logger;
        private readonly ITicketOccupanciesService _ticketOccupanciesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushVantageAzureFunctionOBSTransactions"/> class.
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="reservationTransactionsService">Ticket Occupancies Service</param>
        public PushVantageAzureFunctionPARCSTicketsOccupancies(ILoggerFactory loggerFactory, ITicketOccupanciesService ticketOccupanciesService)
        {
            ArgumentNullException.ThrowIfNull(ticketOccupanciesService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionPARCSTicketsOccupancies>();
            _ticketOccupanciesService = ticketOccupanciesService;
            _logger.LogInformation($"Constructing {nameof(PushVantageAzureFunctionPARCSTicketsOccupancies)}");
        }

        /// <summary>
        /// Function to push parcs ticket occupancies transactions.
        /// </summary>
        /// <param name="req">request data</param>
        /// <returns>IActionResult</returns>
        [Function("PushVantageAzureFunctionPARCSTicketsOccupancies")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/parcs/occupancies")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(PushVantageAzureFunctionPARCSTicketsOccupancies)}");
            var result = await _ticketOccupanciesService.GetOccupanciesAsync();
            _logger.LogInformation($"Executed function {nameof(PushVantageAzureFunctionPARCSTicketsOccupancies)}");

            return new OkObjectResult(result);
        }
    }
}