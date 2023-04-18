using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    public class RevenueAndBudgetForMonth
    {
        public int Year { get; internal set; }
        public int Month { get; internal set; }
        public int BudgetedRevenue { get; internal set; }
        public int Revenue { get; internal set; }
    }
}
