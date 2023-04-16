﻿using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
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
        #endregion

        #region Constructor
        public Repository(IDapperConnection context)
        {
            dapperContext = context;
        }
        #endregion

        #region Entry Methods
        public OccupancyRepository<OccRevenueByProduct> OccupancyRepository
        {
            get
            {
                if (_occupancyRepository == null)
                    _occupancyRepository = new OccupancyRepository<OccRevenueByProduct>(dapperContext);

                return _occupancyRepository;
            }
        }

        public ReservationsRepository<ReservationsByHour> ReservationsRepository
        {
            get
            {
                if (_reservationRepository == null)
                    _reservationRepository = new ReservationsRepository<ReservationsByHour>(dapperContext);

                return _reservationRepository;
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
