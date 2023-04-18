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
    public class DashboardFunctionDailyTransaction
    {
        private readonly ILogger _logger;
        private readonly ITransaction_NewService _transactionService;

        public DashboardFunctionDailyTransaction(ILoggerFactory loggerFactory, ITransaction_NewService transactionService)
        {
            ArgumentNullException.ThrowIfNull(transactionService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionDailyTransaction>();
            _transactionService = transactionService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionDailyTransaction)}");
        }

        [Function("ABM Dashboard - Get Daily Transaction")]
        [OpenApiOperation(operationId: "GetDailyTransaction", tags: new[] { "ABM Dashboard" }, Summary = "Get Daily Transaction", Description = "")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionDailyTransaction), Summary = "Daily Transaction", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "dailytransaction")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionDailyTransaction)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            //Get total occupancy revenue
            var result = await _transactionService.GetTransactonByDays(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionDailyTransaction)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { yearlyOccupancy = result });
        }
    }
}