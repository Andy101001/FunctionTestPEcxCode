using ABMVantage_Outbound_API.DashboardFunctionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public interface IDayReservationService
    {

        Task<IList<DashboardFuctionDayReservation>> GetDaysResversation(DateTime? tranactionDate, string? facilityId, string? levelId, string? parkingProductId);
    }
}
