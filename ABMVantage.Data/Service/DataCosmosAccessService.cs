using ABMVantage.Data.DataAccess;
using ABMVantage.Data.EntityModels;
using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ABMVantage.Data.Service
{
    public class DataCosmosAccessService : IDataCosmosAccessService
    {
        /// <summary>
        /// Factory to generate <see cref="DocsContext"/> instances.
        /// </summary>
        private readonly IDbContextFactory<CosmosDataContext> _factory;

        #region Reveneue and Transaction

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        /// <param name="factory">The factory instance.</param>
        public DataCosmosAccessService(IDbContextFactory<CosmosDataContext> factory) => _factory = factory;

        #region Revene And Transaction
        public async Task<IList<DailyTransaction>> GetTransactonByDays(FilterParam parameters)
        {
           IList<DailyTransaction> dailyTransactions=null;

            try
            {

                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                var result = context.StgRevenues.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var query = result.ToQueryString();
                var data = from d in result select new DailyTransaction { NoOfTransactions = d.NoOfTransactions, WeekDay = d.Weekday };
                dailyTransactions= data.ToList();

            }
            catch (Exception ex) 
            {
                string error=ex.Message;
            }

            return dailyTransactions;
        }

        public async Task<IList<CurrentTransaction>> GetTransactonByHours(FilterParam parameters)
        {
            IList<CurrentTransaction> transactionsByHours = null;

            try
            {

                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();
                //var result2 = context.TransactionByHourss.ToList();

                var result = context.TransactionByHourss.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var data = from d in result select new CurrentTransaction { NoOfTransactions = d.NoOfTransactions, Time = d.Time };
                transactionsByHours = data.ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return transactionsByHours;
        }

        public async Task<IList<MonthlyTransaction>> GetTransactonByMonth(FilterParam parameters)
        {
            IList<MonthlyTransaction> transactionsByMonth = null;

            try
            {

                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();
                //var result2 = context.TransactionByHourss.ToList();

                var result = context.TransactionByMonths.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var data = from d in result select new MonthlyTransaction { NoOfTransactions = d.NoOfTransactions, MonthAsInt=d.MonthAsInt, Year=d.Year};
                transactionsByMonth = data.ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return transactionsByMonth;
        }

        public async  Task<IList<RevenueBudget>> GetRevenueVsBudget(FilterParam parameters)
        {
            IList<RevenueBudget> revenueBudgets = null;

            try
            {

                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();
                //var result2 = context.RevenueVsBudgets.ToList();

                var result = context.RevenueVsBudgets.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var data = from d in result select new RevenueBudget {BudgetedRevenue = d.BudgetedRevenue, Month=d.Month, Revenue=d.Revenue };
                revenueBudgets = data.ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return revenueBudgets;
        }

        public async Task<IList<RevenueByProduct>> GetRevenueByProductByDays(FilterParam parameters)
        {
            IList<RevenueByProduct> revenueBudgets = null;

            try
            {

                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();
                //var result2 = context.RevenueVsBudgets.ToList();

                var result = context.RevenueByProductByDays.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var data = from d in result select new RevenueByProduct {Revenue=d.Revenue, Product=d.Product};
                revenueBudgets = data.ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return revenueBudgets;
        }

        public async Task<IList<BudgetVariance>> GetBudgetVsActualVariance(FilterParam parameters)
        {

            IList<BudgetVariance> budgetVariance = null;

            try
            {

                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();
                //var result2 = context.RevenueVsBudgets.ToList();

                var result = context.BudgetVsActualVariances.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var data = from d in result select new BudgetVariance {Month=d.Month, BgtVariance=d.BgtVariance};
                budgetVariance = data.ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return budgetVariance;
        }

        public async Task<IList<RevenueByDay>> GetRevenueByDays(FilterParam parameters)
        {
            IList<RevenueByDay> revenueByDay = null;

            try
            {
                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();
                //var result2 = context.RevenueVsBudgets.ToList();

                var result = context.RevenueStgByDays.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var data = from d in result select new RevenueByDay {Revenue=d.Revenue,WeekDay=d.WeekDay};
                revenueByDay = data.ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return revenueByDay;
        }

        public async Task<IList<MonthlyRevenue>> GetRevenueByMonths(FilterParam parameters)
        {
            IList<MonthlyRevenue> monthlyRevenue = null;

            try
            {
                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                var result = context.RevenueByMonths.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var data = from d in result select new MonthlyRevenue {Revenue=d.Revenue, Month=d.Month, PreviousYearRevenue=d.PreviousYearRevenue};
                monthlyRevenue = data.ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return monthlyRevenue;
        }

        #endregion

        #endregion

        #region Reservation

        public async Task<IList<ReservationsByHour>> GetHourlyReservations(FilterParam parameters)
        {
            IList<ReservationsByHour> reservationsByHour = null;

            try
            {
                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                var result = context.ReservationsStgByHours.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var data = from d in result select new ReservationsByHour { NoOfReservations = d.NoOfReservation, Time = d.Time };
                reservationsByHour = data.ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return reservationsByHour;
        }

        public async Task<IList<ReservationsByDay>> GetDailyReservations(FilterParam parameters)
        {
            IList<ReservationsByDay> reservationsByDay = null;

            try
            {
                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                var result = context.ReservationsStgByDays.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var data = from d in result select new ReservationsByDay { NoOfReservations = d.NoOfReservations,WeekDay=d.WeekDay};
                reservationsByDay = data.ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return reservationsByDay;
        }

        public async Task<IList<ReservationsByMonth>> GetMonthlyReservations(FilterParam parameters)
        {
            IList<ReservationsByMonth> reservationsByMonth = null;

            try
            {
                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                var result = context.ReservationsStgByMonths.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var data = from d in result select new ReservationsByMonth { NoOfReservations = d.NoOfReservation, Month=d.Month, Fiscal=d.Fiscal};
                reservationsByMonth = data.ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return reservationsByMonth;
        }

        public async Task<IList<ResAvgTicketValue>> GetReservationsAvgTkt(FilterParam parameters)
        {
            IList<ResAvgTicketValue> resAvgTicketValue = null;

            try
            {
                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                var result = context.ReservationStgAvgTicketValues.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId));

                var data = from d in result select new ResAvgTicketValue {Time=d.Time, NoOfTransactions=d.NoOfTransactions };
                resAvgTicketValue = data.ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return resAvgTicketValue;
        }

        #endregion
    }
}
