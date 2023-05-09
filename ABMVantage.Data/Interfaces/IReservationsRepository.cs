using ABMVantage.Data.Models;

namespace ABMVantage.Data.Interfaces
{
    public interface IReservationsRepository
    {
        Task<IEnumerable<ReservationsByDay>> GetDailyReservations(FilterParam inputFilter);
        Task<IEnumerable<ReservationsByHour>> GetHourlyReservations(FilterParam inputFilter);
        Task<IEnumerable<ReservationsByMonth>> GetMonthlyReservations(FilterParam inputFilter);
        Task<IEnumerable<ResAvgTicketValue>> GetReservationsAvgTkt(FilterParam inputFilter);
    }
}