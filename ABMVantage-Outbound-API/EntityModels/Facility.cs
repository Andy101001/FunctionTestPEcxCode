using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    //[Table("DIM_FACILITY")]
    public class Facility
    {
        [JsonProperty("FacilityId")]
        //[Column("FACILITY_ID")]
        public string? FacilityId { get; set; }

        //[Column("FACILITY_NAME")]
        [JsonProperty("FacilityName")]
        public string? FacilityName { get; set; }

        [Column("LOCATION_ID")]
        public int? LocationId { get; set; }

       // [Column("FACILITY_TYPE")]
        [JsonProperty("FacilityType")]
        public string? FacilityType { get; set; }

        //[Column("AREA_TYPE")]
        [JsonProperty("AreaType")]
        public string? AreaType { get; set; }

        [Column("TOTAL_SPACES")]
        public int? TotalSpace { get; set; }

       // [Column("INCLUDE_BU_NUMBER")]
        [JsonProperty("IncludeBusinessUnitNumber")]
        public bool? IncludeBuNumber { get; set; }

        [Column("BU_CODE")]
        public string? BuCode { get; set; }

        //[Column("LAT")]
        [JsonProperty("Latitude")]
        public double? Lat { get; set; }

        //[Column("LONG")]
        [JsonProperty("Longitude")]
        public double? Long { get; set; }

        //[Column("FACILITY_TYPE_ID")]
        [JsonProperty("FacilityTypeId")]
        public string? FacilityTypeId { get; set; }

        
    }
}
