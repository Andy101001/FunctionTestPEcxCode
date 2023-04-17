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
        public Task<IEnumerable<OccRevenueByProduct>> GetTotalOccRevenue(string userId, int customerId)
            => _repository.OccupancyRepository.GetTotalOccRevenue(userId, customerId);
        public Task<IEnumerable<OccWeeklyOccByDuration>> GetWeeklyOccByDuration(string userId, int customerId)
           => _repository.OccupancyRepository.GetWeeklyOccByDuration(userId, customerId);
        public Task<IEnumerable<OccCurrent>> GetOccCurrent(string userId, int customerId)
           => _repository.OccupancyRepository.GetOccCurrent(userId, customerId);
        public Task<IEnumerable<AvgMonthlyOccVsDuration>> GetAvgMonthlyOccVsDuration(string userId, int customerId)
           => _repository.OccupancyRepository.GetAvgMonthlyOccVsDuration(userId, customerId);
        public Task<IEnumerable<YearlyOccupancy>> GetYearlyOccupancy(string userId, int customerId)
           => _repository.OccupancyRepository.GetYearlyOccupancy(userId, customerId);
        #endregion
    }
}
