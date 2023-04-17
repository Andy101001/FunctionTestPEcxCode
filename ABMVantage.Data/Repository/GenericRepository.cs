using ABMVantage.Data.Interfaces;
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

        #region Constructor
        public GenericRepository(IDapperConnection context)
        {
            dapperContext = context;
        }
        #endregion

        #region Public Methods
        public IDbConnection DapperConnection
        {
            get { return dapperContext.GetConnection(); }
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
