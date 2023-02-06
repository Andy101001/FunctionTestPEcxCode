using ABMVantage_Outbound_API.EntityModels;

namespace ABMVantage_Outbound_API.Services
{
    public interface IObsReservationService
    {
        Task<List<Booking>> GetAllReservationsAsync();
        Task<Booking> GetReservationAsync(string id);
    }
}