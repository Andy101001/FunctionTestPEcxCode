﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.DashboardFunctionModels
{
    public class DashboardMonthlyRevenueAndBudget
    {
        public IEnumerable<RevenueAndBudget> MonthlyRevenueAndBudget { get; set; }
    }

    public class RevenueAndBudget
    {
        public string Month { get; set; }
        public int Revenue { get; set; }
        public int BudgetedRevenue { get; set; }
    }
}
