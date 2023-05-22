namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.Interfaces;
    using ABMVantage.Data.Models;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ReservationNTicketService : ServiceBase, IReservationNTicketService
    {
        #region Constructor

        private readonly ILogger<ReservationNTicketService> _logger;
        private readonly IDataCosmosAccessService _cosmosAccessService;
        public ReservationNTicketService(ILoggerFactory loggerFactory, IRepository repository, IDataCosmosAccessService cosmosAccessService)
        {
            ArgumentNullException.ThrowIfNull(repository);
            _logger = loggerFactory.CreateLogger<ReservationNTicketService>();
            _repository = repository;
            _cosmosAccessService = cosmosAccessService;
        }

        #endregion Constructor

        #region Public Methods

        public async Task<IEnumerable<ReservationsByHour>> GetHourlyReservations(FilterParam inputFilter)
        {
            var result = await _cosmosAccessService.GetHourlyReservations(inputFilter);
            return result;
        }

        public async Task<IEnumerable<ReservationsByDay>> GetDailyReservations(FilterParam inputFilter)
        {
            var result = await _cosmosAccessService.GetDailyReservations(inputFilter);
            return result;
        }

        public async Task<IEnumerable<ReservationsByMonth>> GetMonthlyReservations(FilterParam inputFilter)
        {
            var result = await _cosmosAccessService.GetMonthlyReservations(inputFilter);
            return result;
        }

        public async Task<IEnumerable<ResAvgTicketValue>> GetReservationsAvgTkt(FilterParam inputFilter)
        {
            var result = await _cosmosAccessService.GetReservationsAvgTkt(inputFilter);
            return result;
        }

        #region Dad Code
        //public Task<IEnumerable<ReservationsByHour>> GetHourlyReservations(FilterParam inputFilter)
        //    => _repository.ReservationsRepository.GetHourlyReservations(inputFilter);

        //public Task<IEnumerable<ReservationsByDay>> GetDailyReservations(FilterParam inputFilter)
        //    => _repository.ReservationsRepository.GetDailyReservations(inputFilter);

        //public Task<IEnumerable<ReservationsByMonth>> GetMonthlyReservations(FilterParam inputFilter)
        //    => _repository.ReservationsRepository.GetMonthlyReservations(inputFilter);

        //public Task<IEnumerable<ResAvgTicketValue>> GetReservationsAvgTkt(FilterParam inputFilter)
        //    => _repository.ReservationsRepository.GetReservationsAvgTkt(inputFilter);
        #endregion

        #endregion Public Methods
    }
}