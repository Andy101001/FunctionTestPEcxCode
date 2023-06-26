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
        Task<ReservationsByHourList> GetHourlyReservations(FilterParam inputFilter);
        Task<ReservationsByDayList> GetDailyReservations(FilterParam inputFilter);
        Task<ReservationsByMonthList> GetMonthlyReservations(FilterParam inputFilter);
        Task<ResAvgTicketValueList> GetReservationsAvgTkt(FilterParam inputFilter);
    }
}
