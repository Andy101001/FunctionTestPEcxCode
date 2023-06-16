namespace ABMVantage_Outbound_API.Functions
{
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using System.Net;
    using System.Net.Http;

    public class SingleTicketEVCharges
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISingleTicketEVChargesService _singleTicketEVChargesService;

        public SingleTicketEVCharges(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory,  ISingleTicketEVChargesService singleTicketEVChargesService)
        {
            _logger = loggerFactory.CreateLogger<SingleTicketEVCharges>();
            _httpClientFactory = httpClientFactory;
            _singleTicketEVChargesService = singleTicketEVChargesService;
        }

        [Function("ABM Vantage - Get EV Charges for Single Transaction")]
        [OpenApiOperation(operationId: "SingleTransationEVcharges", tags: new[] { "ABM Vantage" }, Summary = "Get EV Charges for Single Transaction", Description = "Get EV Charges by TicketId Or LPN")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SingleTicketEVChargesResponse), Summary = "Get EV Charges for Single Transaction", Description = "Get EV Charges by TicketId Or LPN")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Request", Description = "Invalid Request")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "singletransationevcharges")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation($"Executing function {nameof(SingleTicketEVCharges)}");
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                SingleTicketEVChargesRequest? request = JsonConvert.DeserializeObject<SingleTicketEVChargesRequest>(content);
                if (request != null && (!string.IsNullOrEmpty(request.Lpn) || !string.IsNullOrEmpty(request.TicketId)))
                {
                    var result = await _singleTicketEVChargesService.GetEVCharges(request!);
                    if (result != null && !result.SessionEndTimeInUtc.HasValue)
                    {
                        _logger.LogInformation($"Triggering Close EV Session Event - Mock API Call");
                        result.SessionEndTimeInUtc = DateTime.UtcNow;

                        //Trigger CloseSession event Api Call
                        try
                        {
                            var httpRequestMessage = new HttpRequestMessage(
                                    HttpMethod.Post, "EVAPIRequestUrl")
                            { };
                            /*
                            var httpClient = _httpClientFactory.CreateClient();
                            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                using var contentStream =
                                    await httpResponseMessage.Content.ReadAsStreamAsync();
                            } */
                        }
                        catch{ }
                    }
                    _logger.LogInformation($"Executed function {nameof(SingleTicketEVCharges)}");
                    return new OkObjectResult(result);
                }
                else
                    throw new ArgumentException();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"{nameof(SingleTicketEVCharges)} Missing query parameters {ae.Message}");

                return new BadRequestObjectResult("Missing or invalid query parameters.");
            }
        }
    }
}