using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Models
{
    public class OccRevenueByProduct
    {
        public int ProductId { get; set; }
        public string Product { get; set; }
        public decimal Revenue { get; set; }
    }
}
