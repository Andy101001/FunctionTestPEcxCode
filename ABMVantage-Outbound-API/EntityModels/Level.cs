using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
   
    [Table("DIM_LEVEL")]
    public class Level
    {
        [Column("LEVEL_ID")]
        public string? LevelId { get; set; }

        [Column("FACILITY_ID")]
        public string? FacilityId { get; set; }

        [Column("LEVEL")]
        public int? LevelName { get; set; }

        [Column("TOTAL_SPACES")]
        public int? TotalSpace { get; set; }

        [Column("SUPPORTS_OVERFLOW")]
        public bool? SupportOverflow { get; set; }

        [Column("IS_ROOF")]
        public bool? IsRoof { get; set; }

        [Column("LEVEL_DESCRIPTION")]
        public string? LevelDescription { get; set; }
    }
}
