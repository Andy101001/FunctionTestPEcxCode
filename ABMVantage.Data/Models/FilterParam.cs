using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Models
{
    public class FilterParam
    {
        public string UserId { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<FacilityFilter> Facilities { get; set; }
        public IEnumerable<LevelFilter> ParkingLevels { get; set; }
        public IEnumerable<ProductFilter> Products { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class FacilityFilter
    {
        public string Id { get; set; }
        public string Name { get; set;}
    }

    public class LevelFilter
    {
        public string Id { get; set; }
        public int Level { get; set; }
    }

    public class ProductFilter
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
