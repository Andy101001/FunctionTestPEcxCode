﻿using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using ABMVantage.Data.Service;
using ABMVantage_Outbound_API.DashboardFunctionModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;

namespace ABMVantage_Outbound_API.Functions.RevenueNTransaction
{
    public class DashboardFunctionMonthlyTransaction
    {
        private readonly ILogger _logger;
        private readonly IOccupancyService _occupancyService;

        public DashboardFunctionMonthlyTransaction(ILoggerFactory loggerFactory, IOccupancyService occupancyService)
        {
            ArgumentNullException.ThrowIfNull(occupancyService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionMonthlyTransaction>();
            _occupancyService = occupancyService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionMonthlyTransaction)}");
        }

        [Function("ABM Dashboard - Get Monthly Transaction")]
        [OpenApiOperation(operationId: "GetMonthlyTransaction", tags: new[] { "ABM Dashboard" }, Summary = "Get Monthly Transaction", Description = "")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionMonthlyTransaction), Summary = "Monthly Transaction", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "monthlytransaction")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionMonthlyTransaction)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            //Get total occupancy revenue
            var result = await _occupancyService.GetYearlyOccupancy(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionMonthlyTransaction)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { yearlyOccupancy = result });
        }
    }
}
