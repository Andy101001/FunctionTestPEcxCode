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

    public class LocationFunctionChargerPostition
    {
        private readonly ILogger _logger;

        private readonly IEVChargerLocationService _chargerService;

        public LocationFunctionChargerPostition(ILoggerFactory loggerFactory, IEVChargerLocationService chargerService)
        {
            ArgumentNullException.ThrowIfNull(chargerService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<EVChargerLocationService>();
            _logger.LogInformation($"Constructing {nameof(LocationFunctionChargerPostition)}");
            _chargerService = chargerService;
        }

        [Function("ABM Microsite - GetChargerLocation")]
        [OpenApiOperation(operationId: "GetChargerLocation", tags: new[] { "ABM Microsite" }, Summary = "Get Charger Location", Description = "Gets the charager location By Device Id, latitude and Logitude")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(MSPageLoadRequest), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LocationFunctionChargerPostition), Summary = "Charager Location", Description = "Gets the charage location By Device Id, latitude and Logitude.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "charger")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(LocationFunctionChargerPostition)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            MSPageLoadRequest inputFilter = JsonConvert.DeserializeObject<MSPageLoadRequest>(content);

            if (string.IsNullOrEmpty(content))
            {
                _logger.LogError($"{nameof(LocationFunctionChargerPostition)}  parametrs  are EMPTY OR not supplied!");
                throw new ArgumentNullException("inputFilter");
            }

            var result = await _chargerService.GetChargerLocation(inputFilter);
            _logger.LogInformation($"Executed function {nameof(LocationFunctionChargerPostition)}");

            return new OkObjectResult(new { ApiResponse = result.FirstOrDefault() });
        }
    }
}