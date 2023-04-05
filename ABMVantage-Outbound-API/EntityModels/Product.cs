using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("DIM_PRODUCT")]
    public class Product
    {
        [Column("PARKING_PRODUCT_ID")]
        public string? ProductId { get; set; }

        [Column("FACILITY_ID")]
        public string? FacilityId { get; set; }

        [Column("PRODUCT_NAME")]
        public string? ProductName { get; set; }

        [Column("PRODUCT_DESCRIPTION")]
        public string? ProductDescription { get; set; }

        [Column("IS_ACTIVE")]
        public bool? IsActive { get; set; }
    }
}
