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

    public class FilterData
    {
        public IEnumerable<FacilityData> Facilities { get; set; }
        public IEnumerable<LevelData> Levels { get; set; }
        public IEnumerable<ProductData> Products { get; set; }
    }

    public class FacilityData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class LevelData
    {
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string Id { get; set; }
        public int Level { get; set; }
    }

    public class ProductData
    {
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string LevelId { get; set; }
        public int Level { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ServiceLocations
    {
        public string UserId { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<CustumerBus> BUs { get; set; }
    }

    public class CustumerBus
    {        
        public string Bu { get; set; }
    }

    public class FilterRawData
    {
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string LevelId { get; set; }
        public int Level { get; set; }
    }
}
