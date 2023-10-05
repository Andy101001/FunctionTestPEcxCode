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

        [Column("ParkingProductId")]
        public int ProductId { get; set; }

        [Column("ParkingSpaceCount")]
        public int ParkingSpaceCount { get; set; }

        [Column("TotalOccupiedParkingSpotMinutesForDay")]
        public int TotalOccupancyMinutes { get; set; }

        [Column("Day")]
        public DateTime? Day { get; set; }
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

        [Column("ParkingProductId")]
        public int ProductId { get; set; }

        [Column("ParkingSpaceCount")]
        public int ParkingSpaceCount { get; set; }

        [Column("TotalOccupiedParkingSpotMinutesForMonth")]
        public long TotalOccupancyInMinutes { get; set; }

        [Column("FirstDayOfMonth")]
        public DateTime FirstDayOfMonth { get; set; }

        [Column("NumberOfDaysInMonth")]
        public int NumberOFDaysInMonth { get; set; }
    }

}
