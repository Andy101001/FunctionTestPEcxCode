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
