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
    /// Gets the transactinos for online book reservations
    /// </summary>
    public class PushVantageAzureFunctionOBSTransactions
    {
        private readonly ILogger _logger;
        private readonly IOBSReservationTransactionsService _reservationTransactionsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushVantageAzureFunctionOBSTransactions"/> class.
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="reservationTransactionsService">Reservation Transactions Service</param>
        public PushVantageAzureFunctionOBSTransactions(ILoggerFactory loggerFactory, IOBSReservationTransactionsService reservationTransactionsService)
        {
            ArgumentNullException.ThrowIfNull(reservationTransactionsService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionReservations>();
            _reservationTransactionsService = reservationTransactionsService;
            _logger.LogInformation($"Constructing {nameof(PushVantageAzureFunctionOBSTransactions)}");
        }

        /// <summary>
        /// Function to push online booking reservation transactions.
        /// </summary>
        /// <param name="req">request data</param>
        /// <returns>IActionResult</returns>
        [Function("PushVantageAzureFunctionOBSTransactions")]
        [OpenApiOperation(operationId: "PushVantageAzureFunctionOBSTransactions", tags: new[] { "PushVantageAzureFunctionOBSTransactions" }, Summary = "Get OBS Transations", Description = "This gets an existing OBS Transations.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Summary = "Get OBS Transations", Description = "Get OBS Transations")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "OBS Transations not found", Description = "OBS Transations not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/vantagePortal/obs/transactions")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(PushVantageAzureFunctionOBSTransactions)}");
            var result = await _reservationTransactionsService.GetObsReservationTransactionsAsync();
            _logger.LogInformation($"Executed function {nameof(PushVantageAzureFunctionOBSTransactions)}");

            return new OkObjectResult(result);
        }
    }
}