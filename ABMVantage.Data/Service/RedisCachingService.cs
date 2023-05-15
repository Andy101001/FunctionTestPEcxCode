
using ABMVantage.Data.Interfaces;
using ABMVantage.Data.Models;
using ABMVantage.Data.Repository;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;

namespace ABMVantage.Data.Service
{
    public class RedisCachingService: IRedisCachingService
    {
        private readonly ILogger<RedisCachingService> _logger;
        private readonly IConnectionMultiplexer _cache;
        private readonly IRepository _filterDataRepository;
        /// <summary>
        /// Redis Caching Service
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="cache"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RedisCachingService(ILoggerFactory loggerFactory, IConnectionMultiplexer cache, IRepository repository) 
        {
            _logger = loggerFactory.CreateLogger<RedisCachingService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _filterDataRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IList<StgFilterData>> GetStgFilterData()
        {
            var result = new List<StgFilterData>();

            try
            {
                string key = $"FilterDataKey";
                var redisKey = new RedisKey(key);

                if (!_cache.GetDatabase().KeyExists(redisKey))
                {
                    var resultStg = await _filterDataRepository.FilterDataRepository.GetStsFiltersData();

                    result= resultStg.ToList();
                    // Add it to the redis cache

                   var resultStgJson = JsonConvert.SerializeObject(resultStg);

                    await _cache.GetDatabase().StringSetAsync(key, resultStgJson);
                }
                else
                {
                    //retur data from cache.

                    var data = await _cache.GetDatabase().StringGetAsync(key);

                    result= JsonConvert.DeserializeObject<IList<StgFilterData>>(data).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(RedisCachingService)} has an error! : {ex.Message}");
              
            }

            return result;
        }

        public async Task<IEnumerable<DailyTransaction>> GetStgTransactonByDays(FilterParam inputFilter)
        {
            IEnumerable<DailyTransaction>? result = null;
            IEnumerable<StgDailyTransaction>? resultStg = null;

            string key = $"DailyTransactionKey";
            var redisKey = new RedisKey(key);

            if (!_cache.GetDatabase().KeyExists(redisKey))
            {
                resultStg = await _filterDataRepository.TransactionRepository.GetTransactionByDays(inputFilter);

                var resultStgJson = JsonConvert.SerializeObject(resultStg);

                await _cache.GetDatabase().StringSetAsync(key, resultStgJson);
            }
            else
            {
                //retur data from cache.

                var cacheData = await _cache.GetDatabase().StringGetAsync(key);

                resultStg = JsonConvert.DeserializeObject<IList<StgDailyTransaction>>(cacheData).ToList();
               
            }

            var levels = inputFilter.ParkingLevels.Select(x => x.Id).ToArray();
            var facilityes = inputFilter.Facilities.Select(x => x.Id).ToArray();
            var products = inputFilter.Products.Select(x => x.Id).ToArray();

            ///DOTO need to fix LevelId fiter<Staging table having levelId nulll></Staging>
            var data = resultStg.ToList().Where(x => (levels.Contains(x.LevelId) || x.LevelId == null) && facilityes.Contains(x.FacilityId) && products.Contains(Convert.ToInt32(x.ProductId)));

            result = from d in data
                     select new DailyTransaction { NoOfTransactions = d.NoOfTransactions, WeekDay = d.WeekDay };

            return result.ToList();
        }
    }
}
