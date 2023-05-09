namespace ABMVantage.Data.Tools
{
    using ABMVantage.Data.Interfaces;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class DapperConnection : IDapperConnection
    {
        #region Properties

        private DbConnection conn;
        private bool disposed = false;

        #endregion Properties

        #region Constructor

        public DapperConnection(string connString)
        {
            if (!string.IsNullOrEmpty(connString))
                conn = new SqlConnection(connString);
        }

        #endregion Constructor

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

        #endregion Public Methods
    }
}