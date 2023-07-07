using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABMVantage.Data.EntityModels.SQL
{
    [Table("RptOccupancyAverageForDay")]
    public class InsightsAverageDialyOccupanySQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("ParkingSpaceCount")]
        public int ParkingSpaceCount { get; set; }

        [Column("TotalOccupancy")]
        public int TotalOccupancy { get; set; }

        [Column("Date")]
        public DateTime? Date { get; set; }
    }

    [Table("RptTicketAverageValueByMonthAndProduct")]
    public class InsightsAverageMonthlyTicketValueSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("ProductName")]
        public string? ProductName { get; set; }

        [Column("FirstDayOfMonth")]
        public DateTime FirstDayOfMonth { get; set; }

        [Column("AverageTicketValue")]
        public decimal AverageTicketValue { get; set; }
    }

    [Table("RptOccupancyAverageForMonth")]
    public class InsightsMonthlyParkingOccupancySQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("ParkingSpaceCount")]
        public int ParkingSpaceCount { get; set; }

        [Column("TotalOccupancy")]
        public int TotalOccupancy { get; set; }

        [Column("FirstDayOfMonth")]
        public DateTime FirstDayOfMonth { get; set; }

        [Column("NoOFDaysInMonth")]
        public int NoOFDaysInMonth { get; set; }
    }

    /*
    [Table("RptRevenueAndBudgetByMonth")]
    public class InsightsMonthlyRevenueAndBudgetSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("ProductName")]
        public string? ProductName { get; set; }

        [Column("FirstDayOfMonth")]
        public DateTime FirstDayOfMonth { get; set; }

        [Column("Revenue")]
        public decimal Revenue { get; set; }

        [Column("BudgetedRevenue")]
        public decimal BudgetedRevenue { get; set; }
    }
    */

    [Table("InsightsMonthlyTransaction")]
    public class InsightsMonthlyTransactionSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("ProductName")]
        public string? ProductName { get; set; }

        [Column("FirstDayOfMonth")]
        public DateTime FirstDayOfMonth { get; set; }

        [Column("TransactionCount")]
        public int TransactionCount { get; set; }

    }

    [Table("InsightsTotalRevenue")]
    public class InsightsTotalRevenueSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("Day")]
        public DateTime Day { get; set; }

        [Column("TotalRevenue")]
        public double TotalRevenue { get; set; }

    }
}
