using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels.SQL
{
    [Table("OccupancyVsDuration")]
    public class OccupancyVsDurationSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int? ProductId { get; set; }

        [Column("OccupancyId")]
        public string? OccupancyId { get; set; }

        [Column("Duration")]
        public string? Duration { get; set; }

        [Column("OccupancyEntryDateTimeUtc")]
        public DateTime? OccupancyEntryDateTimeUtc { get; set; }

        [Column("OccupancyExitDateTimeUtc")]
        public DateTime? OccupancyExitDateTimeUtc { get; set; }
    }

    [Table("OccupancyRevenue")]
    public class OccupancyRevenueSQLData
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int? ProductId { get; set; }

        [Column("ProductName")]
        public string? ProductName { get; set; }

        [Column("Amount")]
        public double Amount { get; set; }
    }
}
