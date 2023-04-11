using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("DIM_CUSTOMER_BU")]
    public class DimCustomer
    {


        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("DIM_CUSTOMER_BU_KEY")]
        public int? CustomerBuKey { get; set; }

        [Column("CUSTOMER_ID")]
        public string? CustomerId { get; set; }

        [Column("BU_CODE")]
        public string? BuCode { get; set; }
    }
}
