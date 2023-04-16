namespace ABMVantage_Outbound_API.DashboardFunctionModels
{
    public class DashboardMonthlyAverageTicketValue
    {
        public string Month { get; set; }
        public IEnumerable<AverageTicketValueForMonth> MonthlyAverageTicketValue { get; set; }
    }

    public class AverageTicketValueForMonth
    {
        public string Product { get; set; }
        public int AverageTicketValue { get; set; }
    }

    /// <summary>
    /// Input Parameters for ticket averages
    /// </summary>
    public class TicketPerYearParameters
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate{ get; set; }
        public string FacilityId { get; set; }
        public string LevelId { get; set; }
        public string ParkingProductId { get; set; }
    }
}