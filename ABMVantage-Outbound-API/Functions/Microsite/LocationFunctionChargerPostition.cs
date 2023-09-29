namespace ABMVantage.Microsite.API.Functions.Microsite
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using System.Net;
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Service;
    using ABMVantage.Data.Models;

    public class LocationFunctionChargerInitiate
    {
        private readonly ILogger _logger;

        private readonly IEVChargerLocationService _chargerService;

        public LocationFunctionChargerInitiate(ILoggerFactory loggerFactory, IEVChargerLocationService chargerService)
        {
            ArgumentNullException.ThrowIfNull(chargerService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<EVChargerLocationService>();
            _logger.LogInformation($"Constructing {nameof(LocationFunctionChargerInitiate)}");
            _chargerService = chargerService;
        }

        [Function("ABM Microsite - GetChargerIntiate")]
        [OpenApiOperation(operationId: "GetChargerIntiate", tags: new[] { "ABM Microsite" }, Summary = "Get Charger Intitate", Description = "Gets the charager Intitate By qrcode, TicketNumberOrReservationId and LPN")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(MSCharagerInitiateRequest), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LocationFunctionChargerInitiate), Summary = "Charager Location", Description = "Gets the charage location By Dqrcode, TicketNumberOrReservationId and LPN")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "chargerinitiate")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(LocationFunctionChargerInitiate)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            MSCharagerInitiateRequest inputFilter = JsonConvert.DeserializeObject<MSCharagerInitiateRequest>(content);

            if (string.IsNullOrEmpty(content))
            {
                _logger.LogError($"{nameof(LocationFunctionChargerInitiate)}  parametrs  are EMPTY OR not supplied!");
                throw new ArgumentNullException("inputFilter");
            }

            var result = await _chargerService.GetChargerInitiate(inputFilter);
            _logger.LogInformation($"Executed function {nameof(LocationFunctionChargerInitiate)}");

            return new OkObjectResult(new { ApiResponse = result.FirstOrDefault() });
        }
    }
}