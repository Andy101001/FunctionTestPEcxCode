using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("DIM_PARKING_SPACE")]
    public class DimParkingSpace
    {
        [Column("PARKING_SPACE_ID ")]
        public string? ParkingSpaceId { get; set; }

        [Column("Parking_Product_Id ")]
        public string? ParkingProductId { get; set; }
    }
}
