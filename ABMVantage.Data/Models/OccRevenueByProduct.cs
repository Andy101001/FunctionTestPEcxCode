namespace ABMVantage.Data.Models
{
    public class OccRevenueByProduct
    {
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Revenue { get; set; }
    }

    public class OccWeeklyOccByDuration
    {        
        public string Duration { get; set; }
        public decimal TotalWeeklyOccupancy { get; set; }
    }

    public class OccCurrent
    {
        public string Time { get; set; }
        public Int32 NoOfOccupiedParking { get; set; }
    }

    public class AvgMonthlyOccVsDuration
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public Int32 NoOfVehicles { get; set; }
        public string Duration { get; set; }
    }

    public class YearlyOccupancy
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public string Fiscal { get; set; }
        public Int32 Occupancy { get; set; }
    }
}
