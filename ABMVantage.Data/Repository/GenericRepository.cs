using ABMVantage.Data.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Repository
{
    public class GenericRepository<T> where T : class
    {
        internal IDapperConnection dapperContext;
        private readonly ILogger<GenericRepository<T>> _logger;

        #region Constructor
        public GenericRepository(ILoggerFactory loggerFactory, IDapperConnection context)
        {
            _logger = loggerFactory.CreateLogger<GenericRepository<T>>();
            dapperContext = context;
        }
        #endregion

        #region Public Methods
        public IDbConnection DapperConnection
        {
            get 
            { 
                _logger.LogInformation($"{nameof(DapperConnection)} Created");
                return dapperContext.GetConnection(); 
            }
        }
        #endregion

        #region Bacic Operations
        public async virtual Task<IEnumerable<T>> GetAsync()
        {
            return await Task.FromResult<IEnumerable<T>>(null);
        }
        #endregion
    }
}
