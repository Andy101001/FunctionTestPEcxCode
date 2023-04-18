using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.DashboardFunctionModels
{
    public class DashboardFunctionDefaultDataAccessQueryParameters
    {
        

        public DashboardFunctionDefaultDataAccessQueryParameters() 
        {

        }
        public DashboardFunctionDefaultDataAccessQueryParameters(FilterParam filterParameters)
        {
            this.FromDate = filterParameters.FromDate;
            this.ToDate = filterParameters.ToDate;
            this.FacilityIds = filterParameters.Facilities != null ? filterParameters.Facilities.Select(x=>x.Id) : null;
            this.LevelIds = filterParameters.ParkingLevels != null ? filterParameters.ParkingLevels.Select(x => x.Id) : null;
            this.ParkingProductIds = filterParameters.Products != null ? filterParameters.Products.Select(x => x.Id) : null;
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public IEnumerable<string> FacilityIds { get; set; }
        public IEnumerable<string> LevelIds { get; set; }
        public IEnumerable<int> ParkingProductIds { get; set; }

        public string? FacilityIdsAsCommaDelimitedString
        {
            get
            {
                if (FacilityIds == null)
                {
                    return null;
                }
                return string.Join(",", FacilityIds); //+ (FacilityIds.Any() ? "'" : string.Empty);
            }
        }

        public string? LevelsIdsAsCommaDelimitedString
        {
            get
            {
                if (LevelIds == null)
                {
                    return null;
                }
                return string.Join(",", LevelIds); //+ (LevelIds.Any() ? "'" : string.Empty);
            }
        }

        public string? ParkingProductIdsAsCommaDelimitedString
        {
            get
            {
                if (ParkingProductIds == null)
                {
                    return null;
                }
                return string.Join(" ,", ParkingProductIds);
            }
        }



    }
}
