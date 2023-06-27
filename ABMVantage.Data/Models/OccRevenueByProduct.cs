namespace ABMVantage.Data.Models
{
    public class OccRevenueByProduct
    {
        public string Product { get; set; }
        public double Revenue { get; set; }
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
        public int MonthInt { get; set; }
    }

    public class AvgMonthlyOccVsDuration
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public Int32 NoOfVehicles { get; set; }
        public string Duration { get; set; }
        public DateTime FirstDayOfMonth { get; internal set; }
    }

    public class YearlyOccupancy
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public string Fiscal { get; set; }
        public Int32 Occupancy { get; set; }
        public DateTime FirstDayOfMonth { get; internal set; }
    }

    public class YearlyOccupancyGroupedResult
    {
        public DateTime FirstDayOfMonth { get; set; }
        public Int32 Occupancy { get; set; }
    }


    public class OccVsDurationGroupedResult
    {
        public string? Duration { get; set; }

        public int Year { get; set; }
        public string Month { get; set; }

        public int NoOfVehicles { get; set; }
        public DateTime FirstDayOfMonth { get; internal set; }
    }

    public class OccRevenueByProductList : ModelBase
    {
        public IList<OccRevenueByProduct> OccRevenueByProduc { get; set; }
    }
    public class OccWeeklyOccByDurationList : ModelBase
    {
        public IList<OccWeeklyOccByDuration> OccWeeklyOccByDurations { get; set; }
    }
    public class OccCurrentList : ModelBase
    {
        public IList<OccCurrent> OccCurrents { get; set; } = new List<OccCurrent>();
    }

    public class AvgMonthlyOccVsDurationList : ModelBase
    {
        public IList<AvgMonthlyOccVsDuration> AvgMonthlyOccVsDurations { get; set; } = new List<AvgMonthlyOccVsDuration>();
    }

    public class YearlyOccupancyList : ModelBase
    {
        public IList<YearlyOccupancy> YearlyOccupancies { get; set; } = new List<YearlyOccupancy>();
    }
}

   