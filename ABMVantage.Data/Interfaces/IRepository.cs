using ABMVantage.Data.Models;
using ABMVantage.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Interfaces
{
    public interface IRepository: IDisposable
    {
        OccupancyRepository<OccRevenueByProduct> OccupancyRepository { get; }
    }
}
