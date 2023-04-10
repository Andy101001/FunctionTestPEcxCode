using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.DashboardFunctionModels
{
    public class DashboardMonthlyAverageTicketValue
    {
        public IEnumerable<AverageTicketValueForMonth> MonthlyAverageTicketValue { get; set; }
    }

    public class AverageTicketValueForMonth
    {
        public string Month { get; set; }
        public int AverageTicketValue { get; set; }
    }
}
