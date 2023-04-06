using ABMVantage_Outbound_API.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public interface IDataAccessSqlService
    {
        /// <summary>
        /// Get Products
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Product>> GetProductAsync(string id);

        /// <summary>
        /// Ge tLevels
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Level>> GetLevelAsync(string id);

        /// <summary>
        /// Get Facilites
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Facility>> GetFacilityAsync(string id);
    }
}
