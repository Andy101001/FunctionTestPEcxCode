using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Models
{
    public class ReservationsByHour
    {
        public int NoOfReservations { get; set; }
        public string Time { get; set; }
    }
    public class ReservationsByDay
    {
        public int NoOfReservations { get; set; }
        public string WeekDay { get; set; }
    }
    public class ReservationsByMonth
    {
        public int NoOfReservations { get; set; }
        public string Month { get; set; }
        public string Fiscal { get; set; }

    }
    public class ResAvgTicketValue
    {
        public int NoOfTransactions { get; set; }
        public string Time { get; set; }
    }
}
