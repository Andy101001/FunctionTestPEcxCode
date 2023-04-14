using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    public class TransactionsByMonthAndProduct
    {
        [Column("PRODUCT_NAME")]
        public string? ParkingProduct { get; set; }
        [Column("YEAR")]
        public int Year { get; set; }
        [Column("MONTH")]
        public int Month { get; set; }
        [Column("TRANSACTION_COUNT")]
        public int TransactionCount { get; set; }
    }
}
