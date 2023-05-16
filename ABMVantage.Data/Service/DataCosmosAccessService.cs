using ABMVantage.Data.DataAccess;
using ABMVantage.Data.EntityModels;
using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Service
{
    public class DataCosmosAccessService : IDataCosmosAccessService
    {
        /// <summary>
        /// Factory to generate <see cref="DocsContext"/> instances.
        /// </summary>
        private readonly IDbContextFactory<CosmosDataContext> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        /// <param name="factory">The factory instance.</param>
        public DataCosmosAccessService(IDbContextFactory<CosmosDataContext> factory) => _factory = factory;

        public async Task<IList<DailyTransaction>> GetTransactonByDays(FilterParam parameters)
        {
            using var context = _factory.CreateDbContext();

            var levels = parameters.ParkingLevels.Select(x => x.Id).ToList();
            var facilities = parameters.Facilities.Select(x => x.Id).ToList();
            var products = parameters.Products.Select(x => x.Id).ToList();

            var result =  context.StgRevenues.Where(x => (levels.Contains(x.LevelId) || x.LevelId == null) && facilities.Contains(x.FacilityId) && products.Contains(Convert.ToInt32(x.ProductId)));

            var data = from d in result select new DailyTransaction {NoOfTransactions=d.NoOfTransactions,WeekDay=d.Weekday };

            return data.ToList();
        }
    }
}
