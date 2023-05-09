namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ReservationNTicketService : ServiceBase, IReservationNTicketService
    {
        #region Constructor

        public ReservationNTicketService(IRepository repository)
        {
            ArgumentNullException.ThrowIfNull(repository);
            _repository = repository;
        }

        #endregion Constructor

        #region Public Methods

        public Task<IEnumerable<ReservationsByHour>> GetHourlyReservations(FilterParam inputFilter)
            => _repository.ReservationsRepository.GetHourlyReservations(inputFilter);

        public Task<IEnumerable<ReservationsByDay>> GetDailyReservations(FilterParam inputFilter)
            => _repository.ReservationsRepository.GetDailyReservations(inputFilter);

        public Task<IEnumerable<ReservationsByMonth>> GetMonthlyReservations(FilterParam inputFilter)
            => _repository.ReservationsRepository.GetMonthlyReservations(inputFilter);

        public Task<IEnumerable<ResAvgTicketValue>> GetReservationsAvgTkt(FilterParam inputFilter)
            => _repository.ReservationsRepository.GetReservationsAvgTkt(inputFilter);

        #endregion Public Methods
    }
}