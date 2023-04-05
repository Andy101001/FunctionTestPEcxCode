using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("DIM_FACILITY")]
    public class Facility
    {
        [Column("FACILITY_ID")]
        public string? FacilityId { get; set; }

        [Column("FACILITY_NAME")]
        public string? FacilityName { get; set; }

        [Column("LOCATION_ID")]
        public int? LocationId { get; set; }

        [Column("FACILITY_TYPE")]
        public string? FacilityType { get; set; }

        [Column("AREA_TYPE")]
        public string? AreaType { get; set; }

        [Column("TOTAL_SPACES")]
        public int? TotalSpace { get; set; }

        [Column("INCLUDE_BU_NUMBER")]
        public bool? IncludeBuNumber { get; set; }

        [Column("BU_CODE")]
        public string? BuCode { get; set; }

        [Column("LAT")]
        public double? Lat { get; set; }

        [Column("LONG")]
        public double? Long { get; set; }

        [Column("FACILITY_TYPE_ID")]
        public string? FacilityTypeId { get; set; }

        
    }
}
