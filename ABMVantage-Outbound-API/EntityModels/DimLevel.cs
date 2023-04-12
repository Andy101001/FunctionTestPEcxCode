using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("DIM_LEVEL")]
    public class DimLevel
    {
        [Column("Level_Id")]
        public string? LevelId { get; set; }

        [Column("Level")]
        public int? LavelName { get; set; }

        [Column("facility_Id")]
        public string? FacilityId { get; set; }
    }
}
