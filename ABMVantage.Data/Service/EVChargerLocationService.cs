using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Service
{
    public class EVChargerLocationService : ServiceBase, IEVChargerLocationService
    {
        private readonly ILogger<EVChargerLocationService> _logger;

        public EVChargerLocationService(ILoggerFactory loggerFactory, IRepository repository)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<EVChargerLocationService>();
            _repository = repository;
        }

        public Task<IEnumerable<MSCharagerInitiateResponse>> GetChargerInitiate(MSCharagerInitiateRequest inputFilter)
            => _repository.EVChargerRepository.GetChargerInitiate(inputFilter);

        #region Public Methods

        public Task<IEnumerable<MSPageLoadResponse>> GetChargerLocation(MSPageLoadRequest inputParam)
           => _repository.EVChargerRepository.GetChargerLocation(inputParam);


        #endregion Public Methods
    }
}