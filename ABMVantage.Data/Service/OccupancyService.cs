using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Service
{
    public class OccupancyService : ServiceBase, IOccupancyService
    {
        #region Constructor
        public OccupancyService(IRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Public Methods
        public Task<IEnumerable<OccRevenueByProduct>> GetTotalOccRevenue(FilterParam inputFilter)
            => _repository.OccupancyRepository.GetTotalOccRevenue(inputFilter);
        public Task<IEnumerable<OccWeeklyOccByDuration>> GetWeeklyOccByDuration(FilterParam inputFilter)
           => _repository.OccupancyRepository.GetWeeklyOccByDuration(inputFilter);
        public Task<IEnumerable<OccCurrent>> GetOccCurrent(FilterParam inputFilter)
           => _repository.OccupancyRepository.GetOccCurrent(inputFilter);
        public Task<IEnumerable<AvgMonthlyOccVsDuration>> GetAvgMonthlyOccVsDuration(FilterParam inputFilter)
           => _repository.OccupancyRepository.GetAvgMonthlyOccVsDuration(inputFilter);
        public Task<IEnumerable<YearlyOccupancy>> GetYearlyOccupancy(FilterParam inputFilter)
           => _repository.OccupancyRepository.GetYearlyOccupancy(inputFilter);
        #endregion
    }
}
