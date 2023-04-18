namespace ABMVantage_Outbound_API.Services
{
    using ABMVantage.Data.Models;
    using ABMVantage_Outbound_API.DashboardFunctionModels;

    /// <summary>
    /// Service for all things to do with tickets
    /// </summary>
    public interface ITicketService
    {

        Task<DashboardMonthlyAverageTicketValue> AverageTicketValuePerYear(FilterParam filterParameter);
    }
}