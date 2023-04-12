using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("DIM_FACILITY")]
    public class DimFacility
    {
        [Column("Facility_Id")]
        public string? FacilityId { get; set; }

        [Column("Facility_Name")]
        public string? FacilityName { get; set; }

        [Column("Location_Id")]
        public int? LocationId { get; set; }
    }
}
