using ABMVantage_Outbound_API.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Models
{
    public class FloorDetails
    {
        public IList<Product> Products { get; set; }
        public IList<Level> Levels { get; set; }
        public IList<Facility> Facilities { get; set; }
    }
}
