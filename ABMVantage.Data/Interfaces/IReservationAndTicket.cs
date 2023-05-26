using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Interfaces
{
    public interface IReservationAndTicketService
    {
        Task<IEnumerable<ReservationsByHour>> GetHourlyReservations(FilterParam inputFilter);
        Task<IEnumerable<ReservationsByDay>> GetDailyReservations(FilterParam inputFilter);
        Task<IEnumerable<ReservationsByMonth>> GetMonthlyReservations(FilterParam inputFilter);
        Task<IEnumerable<ResAvgTicketValue>> GetReservationsAvgTkt(FilterParam inputFilter);
    }
}
