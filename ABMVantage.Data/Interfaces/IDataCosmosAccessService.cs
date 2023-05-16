using ABMVantage.Data.EntityModels;
using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Interfaces
{
    public interface  IDataCosmosAccessService
    {
        Task<IList<DailyTransaction>> GetTransactonByDays(FilterParam parameters);
        Task<IList<CurrentTransaction>> GetTransactonByHours(FilterParam parameters);
    }
}
