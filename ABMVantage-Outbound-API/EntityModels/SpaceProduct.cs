using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("SPACE_PRODUCTS")]
    public class SpaceProduct
    {
        [Column("PARKING_SPACE_ID")]
        public string? ParkingSpaceId { get; set; }

        [Column("PARKING_PRODUCT_ID")]
        public int? ParkingProductId { get; set; }
    }
}
