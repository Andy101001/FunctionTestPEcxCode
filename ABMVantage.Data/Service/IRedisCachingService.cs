
using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Service
{
    public interface IRedisCachingService
    {
        Task<IList<StgFilterData>> GetStgFilterData();
        //Task<IEnumerable<DailyTransaction>> GetStgTransactonByDays(FilterParam inputFilter);
    }
}
