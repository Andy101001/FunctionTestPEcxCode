using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels.SQL
{
    [Table("RptOccupancyDetail")]
    public class OccupancyDetail
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ParkingProductid")]
        public string? ProductId { get; set; }

        [Column("BeginningOfHour")]
        public DateTime BeginningOfHour { get; set; }

        [Column("OccupancyForHour")]
        public int? OccupancyForHour { get; set; }
    }
}
