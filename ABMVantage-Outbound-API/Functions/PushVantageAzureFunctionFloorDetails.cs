using System.Net;
using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace ABMVantage_Outbound_API.Functions
{
    public class PushVantageAzureFunctionFloorDetails
    {
        private readonly ILogger _logger;
        private readonly IFloorDetailsService _floorDetailsService;

        public PushVantageAzureFunctionFloorDetails(ILoggerFactory loggerFactory, IFloorDetailsService floorDetailsService)
        {

            ArgumentNullException.ThrowIfNull(floorDetailsService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionFloorDetails>();
            _floorDetailsService = floorDetailsService;
            _logger.LogInformation($"Constructing {nameof(PushVantageAzureFunctionFloorDetails)}");
        }
        /// <summary>
        /// Function to get floor details
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Function("PushVantageAzureFunctionFloorDetails")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Facility), Summary = "Get PARCS Floor details", Description = "Get PARCS Floor details")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "PARCS Floor details not found", Description = "PARCS Floor details not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/parcs/FloorDetails")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(PushVantageAzureFunctionFloorDetails)}");

            string? customerId = req.Query("customerId");


            if (string.IsNullOrEmpty(customerId))
            {
                _logger.LogError($"{nameof(PushVantageAzureFunctionFloorDetails)} Query string  parametr customerId is EMPTY OR not supplied!");
                throw new ArgumentNullException("customerId");
            }

            var result = await _floorDetailsService.GetFloorDetails(customerId);
            _logger.LogInformation($"Executed function {nameof(PushVantageAzureFunctionFloorDetails)}");

            return new OkObjectResult(result);

        }
    }
}
