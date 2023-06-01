namespace ABMVantage.Data.Models
{
    public class ReservationsByHour
    {
        public int? NoOfReservations { get; set; }
        public string Time { get; set; }
        public TimeSpan TimeOfDay { get; internal set; }
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
        
        public TimeSpan TimeOfDay { get; set; }
    }
}
