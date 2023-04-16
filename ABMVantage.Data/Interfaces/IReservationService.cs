using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationsByHour>> GetHourlyReservations(string userId, int CustomerId);
        Task<IEnumerable<ReservationsByDay>> GetDailyReservations(string userId, int CustomerId);
        Task<IEnumerable<ReservationsByMonth>> GetMonthlyReservations(string userId, int CustomerId);
        Task<IEnumerable<ResAvgTicketValue>> GetReservationsAvgTkt(string userId, int CustomerId);
    }
}
