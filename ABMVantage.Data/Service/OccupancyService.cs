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
        #endregion
    }
}
