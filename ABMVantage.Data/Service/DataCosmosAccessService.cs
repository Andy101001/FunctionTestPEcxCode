using ABMVantage.Data.DataAccess;
using ABMVantage.Data.EntityModels;
using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ABMVantage.Data.Service
{
    public class DataCosmosAccessService : IDataCosmosAccessService
    {
        /// <summary>
        /// Factory to generate <see cref="DocsContext"/> instances.
        /// </summary>
        private readonly IDbContextFactory<CosmosDataContext> _factory;
        private readonly IConnectionMultiplexer _cache;

        #region Reveneue and Transaction

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        /// <param name="factory">The factory instance.</param>
        //public DataCosmosAccessService(IDbContextFactory<CosmosDataContext> factory) => _factory = factory;
        public DataCosmosAccessService(IDbContextFactory<CosmosDataContext> factory, IConnectionMultiplexer cache)
        {

            _factory = factory;
            _cache = cache;
        } 
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

                var result = context.RevenueTransactions.Where(x => (facilities.Contains(x.FacilityId) && products.Contains(x.ProductId))).ToList();

                //var result = context.RevenueTransactions.Where(x => (facilities.Contains(x.FacilityId) && products.Contains(x.ProductId))).Take(100).ToList();

                #region chche
                /*
                // Add it to the redis cache
                string key = $"DailyTransactionKey";
                var redisKey = new RedisKey(key);
                var resultDailyTransactionJson = JsonConvert.SerializeObject(result);

                if (!_cache.GetDatabase().KeyExists(redisKey))
                {
                    await _cache.GetDatabase().StringSetAsync(key, resultDailyTransactionJson);
                }
                else
                {
                    var cacheData = await _cache.GetDatabase().StringGetAsync(key);

                   var result2 = JsonConvert.DeserializeObject<IList<DailyTransaction>>(cacheData);
                    dailyTransactions= result2.ToList();
                }
                */

                #endregion

                var finalRestut = result.GroupBy(x => new { x.TransactionDate.Value.DayOfWeek}).Select(g =>
                new DailyTransaction
                {
                    WeekDay = g.Key.DayOfWeek.ToString(),
                    NoOfTransactions=Convert.ToDecimal(g.Count())
                    
                }
                );

                dailyTransactions = finalRestut.ToList();


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

                var result = context.RevenueTransactions.Where(x => (facilities.Contains(x.FacilityId) && products.Contains(x.ProductId))).ToList();
                //var result = context.RevenueTransactions.Where(x => (facilities.Contains(x.FacilityId) && products.Contains(x.ProductId))).Take(100).ToList();

                var finalRestut = result.GroupBy(x => new { x.TransactionDate.Value.TimeOfDay }).Select(g =>
               new CurrentTransaction
               {
                   Time = GetHourAMPM(g.Key.TimeOfDay.ToString("hh")),
                   NoOfTransactions = Convert.ToDecimal(g.Count())

               }
               );

                var finalRestut2 = finalRestut.GroupBy(x => new { x.Time }).Select(g =>
                new CurrentTransaction
                {
                    Time = g.Key.Time,
                    NoOfTransactions = Convert.ToDecimal(g.Count())
                }
                );

                //var data = from d in result select new CurrentTransaction { NoOfTransactions = d.NoOfTransactions, Time = d.Time };
                transactionsByHours = finalRestut2.ToList();

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

                //var result = context.RevenueTransactions.Where(x => (facilities.Contains(x.FacilityId) && products.Contains(x.ProductId)) && x.TransactionDate >= parameters.FromDate && x.TransactionDate <= parameters.ToDate).Take(100).ToList();
                var result = context.RevenueTransactions.Where(x => (facilities.Contains(x.FacilityId) && products.Contains(x.ProductId)) && x.TransactionDate>=parameters.FromDate && x.TransactionDate<=parameters.ToDate).ToList();


                var finalRestut = result.GroupBy(x => new { x.TransactionDate.Value.Year, x.TransactionDate.Value.Month }).Select(g =>
               new MonthlyTransaction
               {
                   Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                   MonthAsInt= g.Key.Month,
                   Year =g.Key.Year,
                   NoOfTransactions = Convert.ToInt32(g.Count())

               }
               );

                transactionsByMonth = finalRestut.ToList();

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
                //var result2 = context.RevenueRevenueVsBudgets.Take(10).ToList();

                //var result = context.RevenueRevenueVsBudgets.Where(x => facilities.Contains(x.FacilityId) && products.Contains(x.ProductId) && x.TransactionDate>=parameters.FromDate && x.TransactionDate<=parameters.ToDate).Take(100).ToList();
                var result = context.RevenueRevenueVsBudgets.Where(x => facilities.Contains(x.FacilityId) && products.Contains(x.ProductId) && x.TransactionDate >= parameters.FromDate && x.TransactionDate <= parameters.ToDate).ToList();

                var finalRestut = result.GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month }).Select(g =>
                  new RevenueBudget
                  {
                      Month=g.Key.Month.ToString(),
                      BudgetedRevenue=g.Sum(x=>x.BudgetedRevenue),
                      Revenue = g.Sum(x => x.Revenue)
                  }
                  );
                revenueBudgets = finalRestut.ToList();

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
                var result2 = context.RevenueByProductByDays.Take(100).ToList();

                //var result = context.RevenueByProductByDays.Where(x => facilities.Contains(x.FacilityId) && products.Contains(x.ProductId) && x.TransactionId >= parameters.FromDate && x.TransactionId <= parameters.ToDate).Take(100).ToList();
                var result = context.RevenueByProductByDays.Where(x => facilities.Contains(x.FacilityId) && products.Contains(x.ProductId) && x.TransactionId >= parameters.FromDate && x.TransactionId <= parameters.ToDate).ToList();

                var finalRestut = result.GroupBy(x => new { x.Product}).Select(g =>
                  new RevenueByProduct
                  {
                      Product=g.Key.Product,
                      Revenue= g.Sum(x => x.Revenue)
                  }
                  );

               // var data = from d in result select new RevenueByProduct {Revenue=d.Revenue, Product=d.Product};
                revenueBudgets = finalRestut.ToList();

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

                var result = context.RevenueBudgetVsActualVariances.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId)).ToList();

                budgetVariance = result.GroupBy(x => new { x.Month }).Select(g =>
                new BudgetVariance
                {
                    Month = g.Key.Month,
                    BgtVariance = g.Sum(x => x.Bgtvariance),
                }
                ).ToList();

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

                //var result = context.RevenueByProductByDays.Where(x => facilities.Contains(x.FacilityId) && products.Contains(x.ProductId) && x.TransactionId >= parameters.FromDate && x.TransactionId <= parameters.ToDate).Take(100).ToList();
                var result = context.RevenueByProductByDays.Where(x => facilities.Contains(x.FacilityId) && products.Contains(x.ProductId) && x.TransactionId >= parameters.FromDate && x.TransactionId <= parameters.ToDate).ToList();

                var finalRestut = result.GroupBy(x => new { x.TransactionId.Value.DayOfWeek }).Select(g =>
                  new RevenueByDay
                  {
                      WeekDay = g.Key.DayOfWeek.ToString(),
                      Revenue = g.Sum(x => x.Revenue)
                  }
                  );

                //var data = from d in result select new RevenueByDay {Revenue=d.Revenue,WeekDay=d.WeekDay};
                revenueByDay = finalRestut.ToList();

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

                //var result = context.RevenueByProductByDays.Where(x => facilities.Contains(x.FacilityId) && products.Contains(x.ProductId) && x.TransactionId >= parameters.FromDate && x.TransactionId <= parameters.ToDate).Take(100).ToList();
                var result = context.RevenueByProductByDays.Where(x => facilities.Contains(x.FacilityId) && products.Contains(x.ProductId) && x.TransactionId >= parameters.FromDate && x.TransactionId <= parameters.ToDate).ToList();

                var finalRestut = result.GroupBy(x => new { x.TransactionId.Value.Month }).Select(g =>
                  new MonthlyRevenue
                  {
                      Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                      Revenue = g.Sum(x => x.Revenue)
                  }
                  );

                //var data = from d in result select new MonthlyRevenue {Revenue=d.Revenue, Month=d.Month, PreviousYearRevenue=d.PreviousYearRevenue};
                monthlyRevenue = finalRestut.ToList();

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
                //var result2 = context.Reservations.ToList();

                var result = context.Reservations.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "" || x.LevelId==null) && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId)).ToList();

                var budgetVariance = result.GroupBy(x => new { x.ProductId, x.BeginningOfHour.Value.TimeOfDay }).Select(g =>
                new ReservationsByHour
                {
                    Time = GetHourAMPM(g.Key.TimeOfDay.ToString("hh")),
                    NoOfReservations = g.Sum(x => x.NoOfReservations),
                }
                );

                reservationsByHour = budgetVariance.ToList();

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

                var result = context.Reservations.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "" || x.LevelId == null) && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId)).ToList();

                var ltResult = result.GroupBy(x => new { x.ProductId, x.BeginningOfHour.Value.DayOfWeek }).Select(g =>
                new ReservationsByDay
                {
                    WeekDay = g.Key.DayOfWeek.ToString(),
                    NoOfReservations = g.Sum(x => x.NoOfReservations),
                }
                );

                
                reservationsByDay = ltResult.ToList();

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
            IList<ReservationsByMonth> reservationsByMonthFinal = new List<ReservationsByMonth>();

            try
            {
                using var context = _factory.CreateDbContext();

                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                var result = context.Reservations.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "" || x.LevelId == null) && facilities.Contains(x.FacilityId) && products.Contains(x.ProductId)).ToList();

                var budgetVariance = result.GroupBy(x => new { x.ProductId, x.BeginningOfHour.Value.Month }).Select(g =>
                new ReservationsByMonth
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                    NoOfReservations = g.Sum(x => x.NoOfReservations),
                    Fiscal= "CURRENT"
                }
                );

                reservationsByMonth = budgetVariance.ToList();

                var result2 = result.Where(x => x.BeginningOfHour.Value.Year == x.BeginningOfHour.Value.AddYears(-1).Year);


                var budgetVariance2 = result.GroupBy(x => new { x.ProductId, x.BeginningOfHour.Value.Month }).Select(g =>
               new ReservationsByMonth
               {
                   Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                   NoOfReservations = g.Sum(x => x.NoOfReservations),
                   Fiscal = "PREVIOUS"
               }
               );

                reservationsByMonthFinal = reservationsByMonth.Concat(budgetVariance2.ToList()).ToList();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return reservationsByMonthFinal;
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

                var result = context.ReservationAvgTickets.Where(x => (levels.Contains(x.LevelId) || x.LevelId == "") && facilities.Contains(x.Facility_Id) && products.Contains(x.ProductId) && x.ReservedEntryDateTimeUtc >= parameters.FromDate && x.ReservedEntryDateTimeUtc <= parameters.ToDate).ToList();

                //var data = from d in result select new ResAvgTicketValue {Time=d.Time, NoOfTransactions=d.NoOfTransactions };
                //resAvgTicketValue = data.ToList();

                var finalResult = result.GroupBy(x => new { x.ReservedEntryDateTimeUtc.Value.TimeOfDay }).Select(g =>
                new ResAvgTicketValue
                {
                    NoOfTransactions = g.Average(x => x.Total),
                    Time = g.Key.TimeOfDay.ToString("hh")
                    

                }
                );
                resAvgTicketValue = finalResult.ToList();


                var result3 = resAvgTicketValue.GroupBy(x => new { x.Time }).Select(g =>
                
                   new ResAvgTicketValue
                   {
                       Time= GetHourAMPM(g.Key.Time),
                       NoOfTransactions=g.Average(x=>x.NoOfTransactions)
                   }
                );
                resAvgTicketValue = result3.ToList();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return resAvgTicketValue;
        }

        private string GetHourAMPM(string hour)
        {
            string hourAMPM = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} {hour}:00:00.000";
            
            var dt =DateTime.Parse(hourAMPM);
            return dt.ToString("hh:mm tt");

        }

        #endregion
    }
}
