using System.Net;
using ABMVantage.Data.Models;
using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ABMVantage_Outbound_API.Functions
{
    public class DashboardFunctionDailyReservationCountByHour
    {
        private readonly ILogger _logger;
        private readonly IReservationService _reservationService;

        public DashboardFunctionDailyReservationCountByHour(ILoggerFactory loggerFactory, IReservationService reservationService)
        {
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyReservationCountByHour>();
            _reservationService = reservationService;
        }

        [Function("ABM Dashboard - Get Reservation count by hour")]
        [OpenApiOperation(operationId: "GetDailyReservationCountByHour", tags: new[] { "ABM Dashboard" }, Summary = "Get Reservation count by hour", Description = "Gets the number of reservations for each hour, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardDailyReservationCountByHour), Summary = "Reservation count by hour", Description = "The number of reservations for each hour, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardMonthlyParkingOccupancy), Summary = "Get Monthly Parking Occupancy", Description = "Gets the average parking occupancy and previous year's occupancy, potentially filtered by facility, level and product.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "reservationscountbyhour")] HttpRequestData req)
        {
            

            try
            {
                _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyReservationCountByHour)}");
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                FilterParam filterParameters = JsonConvert.DeserializeObject<FilterParam>(content);
                var result = await _reservationService.GetHourlyReservationsByProduct(filterParameters);
                _logger.LogInformation($"Executed function {nameof(DashboardFunctionDailyReservationCountByHour)}");
                return new OkObjectResult(result);
            }
            catch (ArgumentException)
            {
                return new BadRequestResult();
            }

    

        }
    }
}
