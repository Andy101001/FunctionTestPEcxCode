using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Interfaces
{
    public interface IDapperConnection : IDisposable
    {
        IDbConnection GetConnection();
    }
}
