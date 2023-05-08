namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.Interfaces;
    using System;

    public class ServiceBase : IDisposable
    {
        #region Variables

        protected IRepository? _repository;
        private bool disposed;

        #endregion Variables

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

        #endregion Public Methods
    }
}