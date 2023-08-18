using ABMVantage.Data.DataAccess;
using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Service
{
    public class DataRefereshService : ServiceBase, IDataRefereshService
    {
        private readonly ILogger<DataRefereshService> _logger;
        private readonly IRedisCachingService _cache;
        private readonly IDbContextFactory<SqlDataContextVTG> _sqlDataContextVTG;


        public DataRefereshService(ILoggerFactory loggerFactory, IRepository repository, IRedisCachingService cache, IDbContextFactory<SqlDataContextVTG> sqlDataContextVTG)
        {
            ArgumentNullException.ThrowIfNull(repository);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<DataRefereshService>();
            _repository = repository;
            _sqlDataContextVTG = sqlDataContextVTG;
            _cache = cache;
        }
        public async Task<IList<DataReferesh>> GetDataRefereshDetails()
        {
            var listDataReferesh = new List<DataReferesh>();
            
            try
            {
                using var sqlContext = _sqlDataContextVTG.CreateDbContext();

                listDataReferesh = sqlContext.RptDataRefereshSQlData.Select(x => new DataReferesh
                {
                    ChartName = x.ChartName.Trim(),
                    DataRefreshDate=x.DataRefreshDate,
                    PageName=x.PageName.Trim()
                }).ToList();
                
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return listDataReferesh;
        }
    }
}
