using ABMVantage.Data.Models;

namespace ABMVantage.Data.Interfaces
{
    public interface IFilterDataRepository
    {
        Task<FilterData> GetFiltersData(ServiceLocations custBus);
        Task<IList<StgFilterData>> GetStsFiltersData();
    }
}