using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Interfaces
{
    public interface IEVChargerLocationRepository
    {
        Task<IEnumerable<MSPageLoadResponse>> GetChargerLocation(MSPageLoadRequest inputParam);
        Task<IEnumerable<MSCharagerInitiateResponse>> GetChargerInitiate(MSCharagerInitiateRequest inputParam);
    }
}
