using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
   
   // [Table("DIM_LEVEL")]
    public class Level
    {
        [JsonProperty("LevelId")]
        //[Column("LEVEL_ID")]
        public string? LevelId { get; set; }

        //DimFacilityFacilityId
        [JsonProperty("DimFacilityFacilityId")]
        public string? DimFacilityFacilityId { get; set; }

        [JsonProperty("FacilityId")]
        //[Column("FACILITY_ID")]
        public string? FacilityId { get; set; }

        [JsonProperty("Level")]
        //[Column("LEVEL")]
        public int? LevelName { get; set; }

        [JsonProperty("TotalSpaces")]
        //[Column("TOTAL_SPACES")]
        public int? TotalSpace { get; set; }

        [JsonProperty("SupportsOverflow")]
        //[Column("SUPPORTS_OVERFLOW")]
        public bool? SupportOverflow { get; set; }

        [JsonProperty("IsRoof")]
        //[Column("IS_ROOF")]
        public bool? IsRoof { get; set; }

        [JsonProperty("LevelDescription")]
        //[Column("LEVEL_DESCRIPTION")]
        public string? LevelDescription { get; set; }
    }
}
