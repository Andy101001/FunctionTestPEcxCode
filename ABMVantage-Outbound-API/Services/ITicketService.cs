namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage_Outbound_API.DashboardFunctionModels;

    /// <summary>
    /// Service for all things to do with tickets
    /// </summary>
    public interface ITicketService
    {

        Task<DashboardMonthlyAverageTicketValue> AverageTicketValuePerYear(DateTime calculationDate, string? facilityId, string? levelId, string? parkingProductId);
    }
}