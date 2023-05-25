namespace ABMVantage.Data.Service
{
    using ABMVantage.Data.Models;
    public interface IDataAccessService
    {

        Task<DailyAverageOccupancy>? GetDailyAverageOccupancy(FilterParam parameters);
    }
}