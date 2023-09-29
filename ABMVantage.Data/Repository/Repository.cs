using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Repository
{
    public class Repository : IRepository
    {
        #region Properties
        readonly IDapperConnection dapperContext;
        bool disposed = false;
        private OccupancyRepository<OccRevenueByProduct> _occupancyRepository;
        private ReservationsRepository<ReservationsByHour> _reservationRepository;
        private FilterDataRepository<FilterData> _filterDataRepository;
        private TransactionRepository<BudgetVariance> _transactionRepository;
        private EVChargerRepository<MSPageLoadResponse> _eVChargerRepository;
        private readonly ILogger<Repository> _logger;
        private readonly ILoggerFactory _loggerFactory;

        #endregion

        #region Constructor
        public Repository(ILoggerFactory loggerFactory,IDapperConnection context)
        {
            dapperContext = context;
            _loggerFactory = loggerFactory;
        }
        #endregion

        #region Entry Methods
        public OccupancyRepository<OccRevenueByProduct> OccupancyRepository
        {
            get
            {
                if (_occupancyRepository == null)
                    _occupancyRepository = new OccupancyRepository<OccRevenueByProduct>(_loggerFactory, dapperContext);

                return _occupancyRepository;
            }
        }

        public ReservationsRepository<ReservationsByHour> ReservationsRepository
        {
            get
            {
                if (_reservationRepository == null)
                    _reservationRepository = new ReservationsRepository<ReservationsByHour>(_loggerFactory, dapperContext);

                return _reservationRepository;
            }
        }
        public FilterDataRepository<FilterData> FilterDataRepository
        {
            get
            {
                if (_filterDataRepository == null)
                    _filterDataRepository = new FilterDataRepository<FilterData>(_loggerFactory, dapperContext);

                return _filterDataRepository;
            }
        }

        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (_transactionRepository == null)
                    _transactionRepository = new TransactionRepository<BudgetVariance>(_loggerFactory, dapperContext);

                return _transactionRepository;
            }
        }

        public IEVChargerLocationRepository EVChargerRepository
        {
            get
            {
                if (_eVChargerRepository == null)
                    _eVChargerRepository = new EVChargerRepository<MSPageLoadResponse>(_loggerFactory, dapperContext);

                return _eVChargerRepository;
            }
        }

        #endregion

        #region Dispose Method
        protected virtual void Dispose(bool dispose)
        {
            if (!disposed)
                if (dispose)
                    if (dapperContext != null)
                        dapperContext.Dispose();

            disposed = true;

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
