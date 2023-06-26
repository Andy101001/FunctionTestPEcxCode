namespace ABMVantage.Data.Models
{
    public class ReservationsByHour
    {
        public int? NoOfReservations { get; set; }
        public string Time { get; set; }
        public object BeginningOfHour { get; internal set; }
    }
    public class ReservationsByDay
    {
        public int NoOfReservations { get; set; }
        public string WeekDay { get; set; }
        public DateTime Date { get; internal set; }
    }
    public class ReservationsByMonth
    {
        public int? NoOfReservations { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string Fiscal {  get; set; }
        public DateTime FirstDayOfMonth { get; internal set; }
    }
    public class ResAvgTicketValue
    {
        public decimal? NoOfTransactions { get; set; }
        public string? Time { get; set; }
        public DateTime Hour { get; internal set; }
    }

    public class ReservationsByHourList:ModelBase
    {
        public IList<ReservationsByHour> ReservationsByHours { get; set; }
    }

    public class ReservationsByDayList : ModelBase
    {
        public IList<ReservationsByDay> ReservationsByDays { get; set; }
    }
    public class ReservationsByMonthList : ModelBase
    {
        public IList<ReservationsByMonth> ReservationsByMonths { get; set; }
    }

    public class ResAvgTicketValueList : ModelBase
    {
        public IList<ResAvgTicketValue> ResAvgTicketValues { get; set; }
    }

    //
}
