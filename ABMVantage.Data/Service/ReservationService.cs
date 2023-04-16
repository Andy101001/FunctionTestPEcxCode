using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Service
{
    public class ReservationService : ServiceBase, IReservationService
    {
        #region Constructor
        public ReservationService(IRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Public Methods
        
        public Task<IEnumerable<ReservationsByHour>> GetHourlyReservations(string userId, int CustomerId)
            => _repository.ReservationsRepository.GetHourlyReservations(userId, CustomerId);
        public Task<IEnumerable<ReservationsByDay>> GetDailyReservations(string userId, int CustomerId)
            => _repository.ReservationsRepository.GetDailyReservations(userId, CustomerId);
        public Task<IEnumerable<ReservationsByMonth>> GetMonthlyReservations(string userId, int CustomerId)
            => _repository.ReservationsRepository.GetMonthlyReservations(userId, CustomerId);
        public Task<IEnumerable<ResAvgTicketValue>> GetReservationsAvgTkt(string userId, int CustomerId)
            => _repository.ReservationsRepository.GetReservationsAvgTkt(userId, CustomerId);

        #endregion
    }
}
