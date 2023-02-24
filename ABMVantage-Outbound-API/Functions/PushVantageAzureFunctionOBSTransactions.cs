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
    public class PushVantageAzureFunctionOBSTransactions
    {
        private readonly ILogger _logger;
        private readonly IOBSReservationTransactionsService _reservationTransactionsService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="reservationTransactionsService">Reservatin Transactions Service</param>
        public PushVantageAzureFunctionOBSTransactions(ILoggerFactory loggerFactory, IOBSReservationTransactionsService reservationTransactionsService)
        {
            ArgumentNullException.ThrowIfNull(reservationTransactionsService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionReservations>();
            _reservationTransactionsService = reservationTransactionsService;
        }

        /// <summary>
        /// Function to push online booking reservation transactions.
        /// </summary>
        /// <param name="req">request data</param>
        /// <returns>IActionResult</returns>
        [Function("PushVantageAzureFunctionOBSTransactions")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "transactions")] HttpRequestData req)
        {
            _logger.LogInformation("Executing function Get Reservation Transactions");
            var result = await _reservationTransactionsService.GetObsReservationTransactions();
            _logger.LogInformation("Executed Function Get Reservatons Transactions");

            return new OkObjectResult(result);
        }
    }
}