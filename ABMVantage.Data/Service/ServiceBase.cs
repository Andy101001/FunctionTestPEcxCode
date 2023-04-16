using ABMVantage.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Service
{
    public class ServiceBase : IDisposable
    {
        #region Variables
        protected IRepository _repository;
        bool disposed;
        #endregion

        #region Public Methods
        protected virtual void Dispose(bool dispose)
        {
            if (!disposed)
                if (dispose)
                    _repository.Dispose();

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
