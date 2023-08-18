using ABMVantage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Interfaces
{
    public interface IDataRefereshService
    {
        Task<IList<DataReferesh>> GetDataRefereshDetails();
    }
}
