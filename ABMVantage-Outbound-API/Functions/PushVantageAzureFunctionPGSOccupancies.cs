namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage_Outbound_API.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using static System.Reflection.Metadata.BlobBuilder;
    using System.Drawing;
    using System.Net;
    using ABMVantage_Outbound_API.EntityModels;

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
        [OpenApiOperation(operationId: "PushVantageAzureFunctionPGSOccupancies", tags: new[] { "PushVantageAzureFunctionPGSOccupancies" }, Summary = "Get Occupancies", Description = "This gets an existing occupancy.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(PgsOccupancy), Summary = "Get Occupancies", Description = "Get Occupancies")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Occupancy not found", Description = "Occupancy not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/pgs/occupancies")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(PushVantageAzureFunctionPGSOccupancies)}");
            var result = await _ticketOccupanciesService.GetOccupanciesAsync();
            _logger.LogInformation($"Executed function {nameof(PushVantageAzureFunctionPGSOccupancies)}");

            return new OkObjectResult(result);
        }
    }
}