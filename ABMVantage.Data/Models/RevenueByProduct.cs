namespace ABMVantage.Data.Models
{
    public class RevenueByProduct
    {
        public string? Product { get; set; }
        public decimal? Revenue { get; set; }
    }

    public class BudgetVariance
    {
        public string? Month { get; set; }
        public decimal? BgtVariance { get; set; }
        public DateTime FirstDayOfMonth { get; internal set; }
    }

    public class CurrentTransaction
    {
        public string? Time { get; set; }
        public decimal NoOfTransactions { get; set; }
        public TimeSpan TimeOfDay { get; set; }
    }

    public class DailyTransaction:ModelBase
    {
        public string? WeekDay { get; set; }
        public decimal NoOfTransactions { get; set; }
        public DateTime TransactionDate { get; internal set; }
    }
    public class StgDailyTransaction
    {
        public string? WeekDay { get; set; }
        public decimal NoOfTransactions { get; set; }
        public string? LevelId { get; set; }
        public string? FacilityId { get; set; }
        public string? ProductId { get; set; }
        public string? ExitDateTimeUtc { get; set; }
        
    }

    public class MonthlyRevenue
    {
        public string? Month { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? PreviousYearRevenue { get; set; }
        public DateTime FirstDayOfMonth { get; internal set; }
    }

    public class MonthlyTransaction
    {
        public int Year { get; set; }
        public string? Month { get; set; }

        public int MonthAsInt { get; set; }
                
        public int NoOfTransactions { get; set; }

    }

    public class CurrentAndPreviousYearMonthlyTransaction
    {
        public string? Month { get; set; }
        public int NoOfTransactions { get; set; }
        public int PreviousYearNoOfTransactions { get; set; }
    }

    public class RevenueBudget
    {
        public string? Month { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? BudgetedRevenue { get; set; }
        public DateTime FirstDayOfMonth { get; internal set; }
    }
    public class RevenueByDay
    {
        public string? WeekDay { get; set; }

        public DateTime Day { get; internal set; }
        public string Product { get; internal set; }
        public IList<Data> Data { get; set; }
    }

    public class RevenueByDayAndProduct
    {
 
        public decimal? Revenue { get; set; }
        public DateTime Day { get; internal set; }
        public string Product { get; internal set; }
    }
       


    public class Data
    {
        public string Product { get; internal set; }
        public decimal? Revenue { get; set; }

    }

}
