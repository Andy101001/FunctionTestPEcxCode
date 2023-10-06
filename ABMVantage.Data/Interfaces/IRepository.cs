using ABMVantage.Data.Models;
using ABMVantage.Data.Repository;

namespace ABMVantage.Data.Interfaces
{
    public interface IRepository : IDisposable
    {
        OccupancyRepository<OccRevenueByProduct> OccupancyRepository { get; }
        ReservationsRepository<ReservationsByHour> ReservationsRepository { get; }

        FilterDataRepository<FilterData> FilterDataRepository { get; }

        ITransactionRepository TransactionRepository { get; }

        IEVChargerLocationRepository EVChargerRepository { get; }
    }
}