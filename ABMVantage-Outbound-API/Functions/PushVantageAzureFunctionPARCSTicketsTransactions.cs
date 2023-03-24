namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage_Outbound_API.EntityModels;
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
    using Microsoft.Extensions.Logging;
    using System.Net;

    /// <summary>
    /// Gets the transactinos for parcs ticket transactions
    /// </summary>
    public class PushVantageAzureFunctionPARCSTicketsTransactions
    {
        private readonly ILogger _logger;
        private readonly IParcsTicketTransactionsService _parcsTicketTransactionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushVantageAzureFunctionPARCSTicketsTransactions"/> class.
        /// </summary>
        /// <param name="loggerFactory">logger</param>
        /// <param name="reservationTransactionsService">Parcs Ticket Transactions Service</param>
        public PushVantageAzureFunctionPARCSTicketsTransactions(ILoggerFactory loggerFactory, IParcsTicketTransactionsService parcsTicketTransactionService)
        {
            ArgumentNullException.ThrowIfNull(parcsTicketTransactionService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionPARCSTicketsTransactions>();
            _parcsTicketTransactionService = parcsTicketTransactionService;
            _logger.LogInformation($"Constructing {nameof(PushVantageAzureFunctionPARCSTicketsTransactions)}");
        }

        /// <summary>
        /// Function to push parcs tickets transactions.
        /// </summary>
        /// <param name="req">request data</param>
        /// <returns>IActionResult</returns>
        [Function("PushVantageAzureFunctionPARCSTicketsTransactions")]
        [OpenApiOperation(operationId: "PushVantageAzureFunctionPARCSTicketsTransactions", tags: new[] { "PushVantageAzureFunctionPARCSTicketsTransactions" }, Summary = "Get PARCS Ticket Transactions", Description = "This gets an existing PARCS Ticket Transactions.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ParcsTicketsTransactions), Summary = "Get PARCS Ticket Transactions", Description = "Get PARCS Ticket Transactions")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "PARCS Ticket Transactions not found", Description = "PARCS Ticket Transactions not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/vantageAzureFunction/parcs/tickets/transactions")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(PushVantageAzureFunctionPARCSTicketsTransactions)}");
            var result = await _parcsTicketTransactionService.GetParcsTicketTransactionsAsync();
            _logger.LogInformation($"Executed function {nameof(PushVantageAzureFunctionPARCSTicketsTransactions)}");

            return new OkObjectResult(result);
        }
    }
}