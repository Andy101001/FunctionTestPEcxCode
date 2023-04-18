using ABMVantage_Outbound_API.DashboardFunctionModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ABMVantage_Outbound_API.Services;
using Microsoft.Azure.Cosmos.Core;
using ABMVantage.Data.Models;

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionMonthlyAverageTicketValue
    {
        private readonly ILogger _logger;
        private readonly ITicketService _ticketService;
        public DashboardFunctionMonthlyAverageTicketValue(ILoggerFactory loggerFactory, ITicketService ticketService)
        {
            _logger = loggerFactory.CreateLogger<DashboardFunctionMonthlyAverageTicketValue>();
            _ticketService = ticketService;
        }

        [Function("ABM Dashboard - Get Monthly Average Ticket Value")]
        [OpenApiOperation(operationId: "GetMonthlyAverageTicketValue", tags: new[] { "ABM Dashboard" }, Summary = "Get Monthly Average Ticket Value", Description = "Gets the monthly average ticket value, potentially filtered by facility, level and product.")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyAverageTicketValue), Summary = "Get Monthly Average Ticket Value", Description = "Gets the monthly average ticket value, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        /*[OpenApiParameter(name: "calculationDate", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "The start date for which monthly average ticket values are returned.", Description = "The start date for which monthly average ticket values are returned.")]
        [OpenApiParameter(name: "facilityId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "An optional facilityId for filtering.", Description = "When a facilityId is provided, only tickets in that facility are included in average. If the facilityId is \"all\" or \"ALL\", or empty, or null, then the tickets are not filtered by facility.")]
        [OpenApiParameter(name: "levelId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "An optional levelId for filtering", Description = "When a levelId is provided, then the tickets included in the average are filtered by level. If the levelId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by level.")]
        [OpenApiParameter(name: "parkingProductId", In = ParameterLocation.Query, Required = false, Type = typeof(string), Summary = "An optional parkingProductId for filtering", Description = "When a parkingProductId is provided, then the tickets used in the average are filtered by parking product. If the parkingProductId is \"all\" or \"ALL\", or empty, or null, then there is no filtering by parking product.")]*/
        /*public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "monthlyaverageticketvalue")] HttpRequestData reg, [FromQuery] DateTime calculationDate, [FromQuery] string? facilityId, [FromQuery] string? levelId, [FromQuery] string? parkingProductId)*/
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "monthlyaverageticketvalue")] HttpRequestData req)
        {

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyReservationCountByHour)}");
            try
            {

                var result = await _ticketService.AverageTicketValuePerYear(filterParameters);

                return new OkObjectResult(result);
            }
            catch (ArgumentException)
            {
                return new BadRequestResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in function {functionName}", nameof(DashboardFunctionMonthlyAverageTicketValue));
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
            

        }
    }
}
