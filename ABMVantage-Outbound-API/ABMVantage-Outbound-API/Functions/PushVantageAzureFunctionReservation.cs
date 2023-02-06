using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ABMVantage_Outbound_API.Functions
{
    public class PushVantageAzureFunctionReservation
    {
        private readonly ILogger _logger;
        private readonly IObsReservationService _obsReservationService;

        public PushVantageAzureFunctionReservation(ILoggerFactory loggerFactory, IObsReservationService obsReservationService)
        {
            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionReservation>();
            _obsReservationService = obsReservationService;
        }

        [Function("PushVantageAzureFunctionOBSReservations")]
        public async Task<IActionResult> PushVantageAzureFunctionOBSReservations([HttpTrigger(AuthorizationLevel.Function, "get", Route = "OBSReservations")] HttpRequestData req)
        {
            _logger.LogInformation("Executing function Get Reservatons");
            List<Booking> reservations = await _obsReservationService.GetAllReservationsAsync();
            _logger.LogInformation("Executed Function Get Reservatons");

            return new OkObjectResult(reservations);
        }

        //[Function("PushVantageAzureFunctionOBSReservation")]
        //public async Task<IActionResult> PushVantageAzureFunctionOBSReservation([HttpTrigger(AuthorizationLevel.Function, "get", Route = "OBSReservations/{id:string?}")] HttpRequestData req, string? id)
        //{
        //    _logger.LogInformation($"Executing function Get Reservatons for id: {id}");
        //    Booking reservation = await _obsReservationService.GetReservationAsync(id);
        //    _logger.LogInformation($"Executed Function Get Reservatons for id: {id}\");");

        //    return new OkObjectResult(reservation);
        //}
    }
}