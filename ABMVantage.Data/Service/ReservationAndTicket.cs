using ABMVantage.Data.DataAccess;
using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ABMVantage.Data.Service
{
    public class ReservationAndTicketService : ServiceBase, IReservationAndTicketService
    {
        #region Constructor

        private readonly ILogger<ReservationAndTicketService> _logger;
        private readonly IDbContextFactory<SqlDataContextVTG> _sqlDataContextVTG;

        public ReservationAndTicketService(ILoggerFactory loggerFactory, IRepository repository, IDbContextFactory<SqlDataContextVTG> sqlDataContextVTG)
        {
            ArgumentNullException.ThrowIfNull(repository);
            _logger = loggerFactory.CreateLogger<ReservationAndTicketService>();
            _repository = repository;
            _sqlDataContextVTG = sqlDataContextVTG;
        }

        #endregion Constructor

        #region Public Methods

        public async Task<ReservationsByHourList> GetHourlyReservations(FilterParam parameters)
        {
            var reservationsByHourList = new ReservationsByHourList();
            reservationsByHourList.ReservationsByHours = new List<ReservationsByHour>();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                var estDateTime = DateTime.UtcNow.AddHours(-4); //The from date is being converted to the current hour EST. This is a temporary hack until we decide how we are doing to manage date times.

                //Requirement:  show the next 6 hours of reservations
                parameters.FromDate= new(estDateTime.Year, estDateTime.Month, estDateTime.Day, estDateTime.Hour, 0,0);
                // it has to add 5 next hours to show 6 record in including 1 one current hour.
                parameters.ToDate = parameters.FromDate.AddHours(6);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.ReservationsSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                    && levels!.Contains(x.LevelId!)
                    && products!.Contains(x.ProductId) && x.BeginningOfHour>= parameters.FromDate && x.BeginningOfHour < parameters.ToDate).ToList()
                    .Select(g =>
                            new ReservationsByHour
                            {
                                BeginningOfHour = g.BeginningOfHour,
                                Time = g.BeginningOfHour.ToString("hh:mm tt"),
                                NoOfReservations = g.NoOfReservations
                            }).OrderBy(x => x.BeginningOfHour).ToList();
                for (var dateAndTime = parameters.FromDate; dateAndTime < parameters.ToDate; dateAndTime = dateAndTime.AddHours(1))
                {
                    var reservationsCount = result.FirstOrDefault(x => x.BeginningOfHour == dateAndTime);
                    if (reservationsCount == null)
                    {
                        reservationsCount = new ReservationsByHour
                        {
                            BeginningOfHour = dateAndTime,
                            Time = dateAndTime.ToString("hh:mm tt"),
                            NoOfReservations = 0
                        };
                    }
                    reservationsByHourList.ReservationsByHours.Add(reservationsCount);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            //to show UI Data for date range text
            reservationsByHourList.FromDate = parameters.FromDate;
            reservationsByHourList.ToDate = parameters.ToDate;
            return reservationsByHourList;
        }

        public async Task<ReservationsByDayList> GetDailyReservations(FilterParam parameters)
        {
            var reservationsByDay = new ReservationsByDayList();
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                //Requirement: next 7 days of reservations (including current day) 
                parameters.FromDate = DateTime.Today;
                parameters.ToDate = parameters.FromDate.AddDays(7);

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.ReservationsSpanningDaySQLData.Where(x => facilities!.Contains(x.FacilityId!)
                    && levels!.Contains(x.LevelId!)
                    && products!.Contains(x.ProductId) && x.BeginningOfDay >= parameters.FromDate && x.BeginningOfDay <= parameters.ToDate).ToList();
                    /*var groupedResult= result.GroupBy(x => new { x.ProductId, x.BeginningOfHour.Date }).Select(g =>
                        new ReservationsByDay
                        {
                            Date = g.Key.Date, 
                            WeekDay = g.Key.Date.DayOfWeek.ToString(),
                            NoOfReservations = g.Max(x => x.NoOfReservations),
                        }).OrderBy(x =>x.Date).ToList();*/

                for (var day = parameters.FromDate; day < parameters.ToDate; day = day.AddDays(1))
                {
                    var reservationsSpanningDay = result.FirstOrDefault(x => x.BeginningOfDay == day);
                    var reservationsCount = reservationsSpanningDay != null ? new ReservationsByDay { Date = day, NoOfReservations = reservationsSpanningDay.NoOfReservations, WeekDay = day.DayOfWeek.ToString() } : null;
                    if (reservationsCount == null)
                    {
                        reservationsCount = new ReservationsByDay
                        {
                            Date = day,
                            WeekDay = day.DayOfWeek.ToString(),
                            NoOfReservations = 0
                        };
                    }
                    reservationsByDay.ReservationsByDays.Add(reservationsCount);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            //to show UI Data for date range text
            reservationsByDay.FromDate = parameters.FromDate;
            reservationsByDay.ToDate = parameters.ToDate;
           
            return reservationsByDay;
        }

        public async Task<ReservationsByMonthList> GetMonthlyReservations(FilterParam parameters)
        {
            var reservationsByMonthList = new ReservationsByMonthList();
            //Requirement: The chart accurately displays the number of reservations that have actually started in each month within a static 13-month period (January to January).
            var fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var toDate = fromDate.AddMonths(13);


            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();


                using var sqlContext = _sqlDataContextVTG.CreateDbContext();

                var currentYearResult = sqlContext.ReservationsSpanningMonthSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                   && levels!.Contains(x.LevelId!)
                   && products!.Contains(x.ProductId) && x.BeginningOfMonth >= fromDate && x.BeginningOfMonth <= toDate)
                   .Select(x =>
                        new ReservationAndTicketGroupedResult
                        {
                            FirstDayOfMonth = x.BeginningOfMonth,
                            NoOfReservations = x.NoOfReservations
                        }
                   ).ToList();


                var previousYearResult = sqlContext.ReservationsSpanningMonthSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                   && (levels!.Contains(x.LevelId!) || x.LevelId == string.Empty || x.LevelId == null)
                   && products!.Contains(x.ProductId) && x.BeginningOfMonth >= fromDate.AddYears(-1) && x.BeginningOfMonth <= toDate.AddYears(-1))
                 .Select(x =>
                        new ReservationAndTicketGroupedResult
                        {
                            FirstDayOfMonth = x.BeginningOfMonth,
                            NoOfReservations = x.NoOfReservations
                        }
                   ).ToList();

                for (var firstDayOfMonth = fromDate; firstDayOfMonth < toDate; firstDayOfMonth = firstDayOfMonth.AddMonths(1))
                {
                    var currentYearReservationsCount = currentYearResult.FirstOrDefault(x => x.FirstDayOfMonth == firstDayOfMonth);
                    if (currentYearReservationsCount == null)
                    {
                        currentYearReservationsCount = new ReservationAndTicketGroupedResult
                        {
                            FirstDayOfMonth = firstDayOfMonth,
                            NoOfReservations = 0
                        };
                    }
                    reservationsByMonthList.ReservationsByMonths.Add(new ReservationsByMonth
                    {
                        FirstDayOfMonth = currentYearReservationsCount.FirstDayOfMonth,
                        Fiscal = "CURRENT",
                        Year = currentYearReservationsCount.FirstDayOfMonth.Year,
                        NoOfReservations = currentYearReservationsCount.NoOfReservations,
                        Month = currentYearReservationsCount.FirstDayOfMonth.ToString("MMM")
                    });

                    var previousYearReservationsCount = previousYearResult.FirstOrDefault(x => x.FirstDayOfMonth == firstDayOfMonth.AddYears(-1));
                    if (previousYearReservationsCount == null)
                    {
                        previousYearReservationsCount = new ReservationAndTicketGroupedResult
                        {
                            FirstDayOfMonth = firstDayOfMonth,
                            NoOfReservations = 0
                        };
                    }
                    reservationsByMonthList.ReservationsByMonths.Add(new ReservationsByMonth
                    {
                        FirstDayOfMonth = previousYearReservationsCount.FirstDayOfMonth,
                        Fiscal = "PREVIOUS",
                        Year = previousYearReservationsCount.FirstDayOfMonth.Year,
                        NoOfReservations = previousYearReservationsCount.NoOfReservations,
                        Month = previousYearReservationsCount.FirstDayOfMonth.ToString("MMM")
                    });
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            //to show UI Data for date range text
            reservationsByMonthList.FromDate = fromDate;
            reservationsByMonthList.ToDate = toDate;

            return reservationsByMonthList;
        }

        public async Task<ResAvgTicketValueList> GetReservationsAvgTkt(FilterParam parameters)
        {
            ResAvgTicketValueList resAvgTicketValue = new ResAvgTicketValueList();
            var currentDateTimeEst = DateTime.UtcNow.AddHours(-4);
            var fromDate = new DateTime(currentDateTimeEst.Year, currentDateTimeEst.Month, currentDateTimeEst.Day, currentDateTimeEst.Hour, 0, 0);
            var toDate = fromDate.AddDays(1);
            try
            {
                var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
                var facilities = parameters.Facilities.Select(x => x.Id).ToList();
                var products = parameters.Products.Select(x => x.Id).ToList();

                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                var result = sqlContext.ReservationsSQLData.Where(x => facilities!.Contains(x.FacilityId!)
                   && levels!.Contains(x.LevelId!)
                   && products!.Contains(x.ProductId)
                   && x.BeginningOfHour >= fromDate && x.BeginningOfHour < toDate).Select(r =>
                   new ResAvgTicketValue
                   {
                       Hour = r.BeginningOfHour,
                       NoOfTransactions = (r.TotalTicketValue/r.NoOfReservations),
                       Time = r.BeginningOfHour.ToString("hh:mm tt")
                   }).OrderBy(x=>x.Hour);




                resAvgTicketValue.ResAvgTicketValues = result.ToList();
                /*var result3 = resAvgTicketValue.GroupBy(x => new { x.Time }).Select(g =>
                    new ResAvgTicketValue    
                    {
                       Time = GetHourAMPM(g.Key.Time),
                       NoOfTransactions = g.Average(x => x.NoOfTransactions)
                   }
                );*/
   
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            //to show UI Data for date range text
            resAvgTicketValue.FromDate = parameters.FromDate;
            resAvgTicketValue.ToDate = parameters.ToDate;

            return resAvgTicketValue;
        }

        #endregion Public Methods

        private string GetHourAMPM(string hour)
        {
            string hourAMPM = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} {hour}:00:00.000";

            var dt = DateTime.Parse(hourAMPM);
            return dt.ToString("hh:mm tt");
        }

    }

    public class ReservationAndTicketGroupedResult
    {
        public DateTime FirstDayOfMonth { get; set; }
        public int NoOfReservations { get; set; }
    }
}
