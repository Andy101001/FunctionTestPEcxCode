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
        /// ctor
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="reservationTransactionsService">Reservatin Transactions Service</param>
        public PushVantageAzureFunctionPARCSTicketsOccupancies(ILoggerFactory loggerFactory, ITicketOccupanciesService ticketOccupanciesService)
        {
            ArgumentNullException.ThrowIfNull(ticketOccupanciesService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionReservations>();
            _ticketOccupanciesService = ticketOccupanciesService;
        }

        /// <summary>
        /// Function to push parcs ticket occupancies transactions.
        /// </summary>
        /// <param name="req">request data</param>
        /// <returns>IActionResult</returns>
        [Function("PushVantageAzureFunctionPARCSTicketsOccupancies")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "occupancies")] HttpRequestData req)
        {
            _logger.LogInformation("Executing function Get parcs ticket occupancies");
            var result = await _ticketOccupanciesService.GetOccupanciesAsync();
            _logger.LogInformation("Executed Function Get parcs ticket occupancies");

            return new OkObjectResult(result);
        }
    }
}