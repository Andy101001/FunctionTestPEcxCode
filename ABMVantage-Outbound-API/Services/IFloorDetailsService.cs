using ABMVantage_Outbound_API.EntityModels;
using ABMVantage_Outbound_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.Services
{
    public interface IFloorDetailsService
    {
        /// <summary>
        /// Get AllProduct
        /// </summary>
        /// <returns>product list</returns>
        Task<List<Product>> GetAllProductAsync(string id);
        /// <summary>
        /// Get All Levels
        /// </summary>
        /// <returns>Level list</returns>
        Task<List<Level>> GetAllLevelAsync(string id);
        /// <summary>
        /// Get All Facility
        /// </summary>
        /// <returns>Facility list</returns>
        Task<List<Facility>> GetAllFacilityAsync(string id);

        /// <summary>
        /// Get floor data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FloorDetails> GetFloorDetails(string id);
    }
}
