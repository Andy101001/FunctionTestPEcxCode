using ABMVantage.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Tools
{
    public class DapperConnection : IDapperConnection
    {
        #region Properties
        private DbConnection conn;
        bool disposed = false;
        #endregion

        #region Constructor
        public DapperConnection(string connString)
        {
            if (!string.IsNullOrEmpty(connString))
                conn = new SqlConnection(connString);
        }
        #endregion

        #region Public Methods
        public IDbConnection GetConnection()
        {
            return conn;
        }

        protected virtual void Dispose(bool dispose)
        {
            if (!disposed)
                if (dispose)
                    conn.Dispose();

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
