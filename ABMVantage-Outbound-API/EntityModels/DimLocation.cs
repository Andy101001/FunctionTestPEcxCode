using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("DIM_LOCATION")]
    public class DimLocation
    {
        [Column("Location_Id")]
        public int? LocationId { get; set; }

        [Column("BU_CODE")]
        public string? BuCode { get; set; }
    }
}
