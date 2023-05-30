using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Configuration
{
    public class DashboardFunctionSettings
    {
        public DashboardFunctionSettings() 
        {
            MinimumValidCalculationDate = new DateTime(1900, 1, 1);
        }
        public DateTime MinimumValidCalculationDate { get; set; }

    }
}
