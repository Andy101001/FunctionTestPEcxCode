using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.DashboardFunctionModels
{
    public class DashboardFunctionDefaultDataAccessQueryParameters
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? FacilityId { get; set; }
        public string? LevelId { get; set; }
        public string? ParkingProductId { get; set; }   
    }
}
