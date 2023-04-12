using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("DIM_PRODUCT")]
    public class DimProduct
    {
        [Column("PARKING_PRODUCT_ID")]
        public int? ProductId { get; set; }

        [Column("PRODUCT_NAME")]
        public string? ProductName { get; set; }

    }
}
