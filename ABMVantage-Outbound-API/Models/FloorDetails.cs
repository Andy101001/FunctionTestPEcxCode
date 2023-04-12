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
        public IList<DimProduct> Products { get; set; }
        public IList<DimLevel> Levels { get; set; }
        public IList<DimFacility> Facilities { get; set; }
    }
}
