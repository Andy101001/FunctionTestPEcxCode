using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ABMVantage_Outbound_API.Functions
{
    public class PushVantageAzureFunctionReservations
    {
        private readonly ILogger _logger;
        private readonly IObsReservationService _obsReservationService;

        public PushVantageAzureFunctionReservations(ILoggerFactory loggerFactory, IObsReservationService obsReservationService)
        {
            ArgumentNullException.ThrowIfNull(obsReservationService);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _logger = loggerFactory.CreateLogger<PushVantageAzureFunctionReservations>();
            _obsReservationService = obsReservationService;
        }

        [Function("PushVantageAzureFunctionOBSReservations")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "OBSReservations")] HttpRequestData req)
        {
            _logger.LogInformation("Executing function Get Reservatons");
            List<Booking> reservations = await _obsReservationService.GetAllReservationsAsync();
            _logger.LogInformation("Executed Function Get Reservatons");

            return new OkObjectResult(reservations);
        }
    }
}