namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.DataAccess;
    using ABMVantage.Data.EntityModels.SQL;
    using ABMVantage.Data.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;

    public class LPNImageCaptureMetadataService : ILPNImageCaptureMetadataService
    {
        private readonly ILogger<LPNImageCaptureMetadataService> _logger;
        private readonly IDbContextFactory<SqlDataContextVTG> _sqlDataContextVTG;

        public LPNImageCaptureMetadataService(ILoggerFactory loggerFactory, IDbContextFactory<SqlDataContextVTG> sqlDataContextVTG)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<LPNImageCaptureMetadataService>();
            _sqlDataContextVTG = sqlDataContextVTG;
        }

        public bool Insert(LPNImageCaptureMetaData? metadata)
        {
            var result = false;
            try
            {
                using var sqlContext = _sqlDataContextVTG.CreateDbContext();
                LPNImageCaptureMetaData? itemFound = sqlContext.LPNImageCaptureMetaData.Where(x => x.UID == metadata!.UID).FirstOrDefault();
                if (itemFound != null)
                {
                    //Delete if existing
                    sqlContext.LPNImageCaptureMetaData.Remove(itemFound!);
                }
                
                sqlContext.LPNImageCaptureMetaData.AddAsync(metadata!);
                int rowCount = sqlContext.SaveChanges();
                if (rowCount > 0) { result = true; }
            }
            catch(Exception ex) { string error = ex.Message; }
            return result;
        }
    }
}