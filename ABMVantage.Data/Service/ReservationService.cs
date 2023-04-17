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
        
        public Task<IEnumerable<ReservationsByHour>> GetHourlyReservations(FilterParam inputFilter)
            => _repository.ReservationsRepository.GetHourlyReservations(inputFilter);
        public Task<IEnumerable<ReservationsByDay>> GetDailyReservations(FilterParam inputFilter)
            => _repository.ReservationsRepository.GetDailyReservations(inputFilter);
        public Task<IEnumerable<ReservationsByMonth>> GetMonthlyReservations(FilterParam inputFilter)
            => _repository.ReservationsRepository.GetMonthlyReservations(inputFilter);
        public Task<IEnumerable<ResAvgTicketValue>> GetReservationsAvgTkt(FilterParam inputFilter)
            => _repository.ReservationsRepository.GetReservationsAvgTkt(inputFilter);

        #endregion
    }
}
