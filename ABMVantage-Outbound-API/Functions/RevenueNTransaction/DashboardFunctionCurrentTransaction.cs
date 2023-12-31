﻿namespace ABMVantage_Outbound_API.Functions.RevenueNTransaction
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

    public class DashboardFunctionCurrentTransaction
    {
        private readonly ILogger _logger;
        private readonly ITransaction_NewService _transactionService;
        private readonly IRevenueAndTransactionService _revenueAndTransactionService;

        public DashboardFunctionCurrentTransaction(ILoggerFactory loggerFactory, ITransaction_NewService transactionService, IRevenueAndTransactionService revenueAndTransactionService)
        {
            ArgumentNullException.ThrowIfNull(transactionService);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DashboardFunctionCurrentTransaction>();
            _transactionService = transactionService;
            _revenueAndTransactionService = revenueAndTransactionService;
            _logger.LogInformation($"Constructing {nameof(DashboardFunctionCurrentTransaction)}");
            _revenueAndTransactionService = revenueAndTransactionService;
        }

        [Function("ABM Dashboard - Get CurrentTransaction")]
        [OpenApiOperation(operationId: "GetCurrentTransaction", tags: new[] { "ABM Dashboard" }, Summary = "Get Current Transaction", Description = "")]
        [OpenApiRequestBody(contentType: "json", bodyType: typeof(FilterParam), Description = "Parameters")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DashboardFunctionCurrentTransaction), Summary = "Current Transaction", Description = "")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Filter Parameters", Description = "Invalid FilterParameters")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "currenttransaction")] HttpRequestData req)
        {
            _logger.LogInformation($"Executing function {nameof(DashboardFunctionCurrentTransaction)}");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            FilterParam inputFilter = JsonConvert.DeserializeObject<FilterParam>(content);

            //Get total occupancy revenue
            //var result = await _transactionService.GetTranacionByHours(inputFilter);
            var result = await _revenueAndTransactionService.GetTransacionByHours(inputFilter);
            _logger.LogInformation($"Executed function {nameof(DashboardFunctionCurrentTransaction)}");

            //Just to make out json as required to UI
            return new OkObjectResult(new { APIResponse = result });
        }
    }
}