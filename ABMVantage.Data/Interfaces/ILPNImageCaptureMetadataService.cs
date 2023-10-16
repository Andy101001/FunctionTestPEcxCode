using ABMVantage.Data.EntityModels.SQL;
using ABMVantage.Data.Models;
using ABMVantage.Data.Models.DashboardModels;

namespace ABMVantage.Data.Interfaces
{
    public interface ILPNImageCaptureMetadataService
    {
        bool Insert(LPNImageCaptureMetaData? metadata);
    }
}