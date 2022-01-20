using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eauction_Seller_API.DataAccess
{
    public interface ICacheAdapter
    {
        Task<T> Get<T>(string key) where T : class;

        Task Set<T>(string key, string value) where T : class;
    }
}
