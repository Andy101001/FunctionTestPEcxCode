namespace ABMVantage_Outbound_API.DashboardFunctionModels
{
    public class DashboardMonthlyAverageTicketValue
    {
        public IEnumerable<AverageTicketValueForMonth> Response { get; set; }
    }

    public class AverageTicketValueForMonth
    {
        public string Month { get; set; }
        public IEnumerable<TicketValueAverage> Data { get; set; }

    }

    public class TicketValueAverage
    {
        public string ParkingProduct { get; set; }
        public decimal AverageTicketValue { get; set; }
    }   

}