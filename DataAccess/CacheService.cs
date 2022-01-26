using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eauction_Seller_API.DataAccess
{
    public class CacheService: ICacheService
    {
        private readonly IDistributedCache _distrCache;

        public CacheService(IDistributedCache cache)
        {
            _distrCache = cache;
        }
        public async Task<T> Get<T>(string key) where T : class
        {
            var cacheResponse = await _distrCache.GetStringAsync(key);
            dynamic result = null;
            if (cacheResponse != null)
                result = JsonConvert.DeserializeObject<T>(cacheResponse);
            return result;
        }

        public async Task Set<T>(string key, string value) where T : class
        {
            await _distrCache.SetStringAsync(key, value);
        }
    }
}
