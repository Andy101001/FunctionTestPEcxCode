using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Models
{
    public class DashboardModels
    {
        public class DashboardDailyReservationCountByHour
        {
            public DashboardDailyReservationCountByHour()
            {
                ReservationsByHour = new List<HourlyReservationCount>();
            }
            public IEnumerable<HourlyReservationCount> ReservationsByHour { get; set; }
        }

        public class ReservationsForProductAndHour
        {
            public DateTime Hour { get; set; }
            public string? Product { get; set; }
            public int ReservationCount { get; set; }
        }

        public class HourlyReservationCount
        {
            public string ReservationTime { get; set; }
            public IEnumerable<ReservationsByProduct> Data { get; set; }
        }

        public class ReservationsByProduct
        {
            public string Product { get; set; } = string.Empty;
            public int NoOfReservations { get; set; }
        }

        public class DashboardMonthlyRevenueAndBudget
        {
            public IEnumerable<RevenueAndBudget> MonthlyRevenueAndBudget { get; set; } = new List<RevenueAndBudget>();
        }

        public class RevenueAndBudgetForMonth
        {
            public int Year { get; internal set; }
            public int Month { get; internal set; }
            public int BudgetedRevenue { get; internal set; }
            public int Revenue { get; internal set; }
        }

        public class RevenueAndBudget
        {
            public string Month { get; set; } = string.Empty;
            public int Revenue { get; set; }
            public int BudgetedRevenue { get; set; }
        }

        public class DashboardMonthlyParkingOccupancy
        {
            public IEnumerable<ParkingOccupancy> MonthlyParkingOccupancy { get; set; } = new List<ParkingOccupancy>();
        }

        public class OccupancyByMonth
        {
            public int Month { get; set; }
            public int Year { get; set; }
            public int OccupancyInteger { get; set; }
            public decimal OccupancyPercentage { get; set; }

        }

        public class ParkingOccupancy
        {
            public string Month { get; set; } = string.Empty;
            public int OccupancyInteger { get; set; }
            public decimal OccupancyPercentage { get; set; }
            public int PreviousYearOccupancyInteger { get; set; }
            public decimal PreviousYearOccupancyPercentage { get; set; }
        }

        public class DashboardMonthlyTransactionCount
        {
            public IEnumerable<TransactionCountForMonth> MonthlyTransactions { get; set; } = new List<TransactionCountForMonth>();
        }

        public class TransactionsByMonthAndProduct
        {
            public string? ParkingProduct { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public int TransactionCount { get; set; }
        }

        public class TransactionCountForMonth
        {
            public string Month { get; set; } = string.Empty;
            public IEnumerable<TransactionsForProduct> Data { get; set; } = new List<TransactionsForProduct>();
        }

        public class TransactionsForProduct
        {
            public string? Product { get; set; }
            public int NoOfTransactions { get; set; }
        }

        public class DashboardMonthlyAverageTicketValue
        {
            public IEnumerable<AverageTicketValueForMonth> Response { get; set; } = new List<AverageTicketValueForMonth>();
        }

        public class AverageTicketValueForMonth
        {
            public string Month { get; set; } = string.Empty;
            public IEnumerable<TicketValueAverage> Data { get; set; } = new List<TicketValueAverage>();

        }

        public class TicketValueAverage
        {
            public string ParkingProduct { get; set; } = string.Empty;
            public decimal AverageTicketValue { get; set; }
        }

    }
}
