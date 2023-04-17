using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Models
{
    public class RevenueByProduct
    {
        public string? Product { get; set; }
        public decimal Revenue { get; set; }
    }

    public class BudgetVariance
    {
        public string? Month { get; set; }
        public decimal BgtVariance { get; set; }
    }

    public class CurrentTransaction
    {
        public string? Time { get; set; }
        public decimal NoOfTransactions { get; set; }
    }

    public class DailyTransaction
    {
        public string? WeekDay { get; set; }
        public decimal NoOfTransactions { get; set; }
    }

    public class MonthlyRevenue
    {
        public string? Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal PreviousYearRevenue { get; set; }
    }

    public class MonthlyTransaction
    {
        public string? Month { get; set; }
        public int NoOfTransactions { get; set; }
        public int PreviousYearNoOfTransactions { get; set; }
    }

    public class RevenueBudget
    {
        public string? Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal BudgetedRevenue { get; set; }
    }
    public class RevenueByDay
    {
        public string? WeekDay { get; set; }
        public decimal Revenue { get; set; }
       
    }

}
