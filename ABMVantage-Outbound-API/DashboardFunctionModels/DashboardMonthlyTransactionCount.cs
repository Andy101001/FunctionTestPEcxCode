﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.DashboardFunctionModels
{
    public class DashboardMonthlyTransactionCount
    {
        public IEnumerable<TransactionCountForMonth> MonthlyTransactions { get; set; }
    }

    public class TransactionCountForMonth
    {
        public string Month { get; set; }
        public int TransactionCount { get; set; }
    }
}
