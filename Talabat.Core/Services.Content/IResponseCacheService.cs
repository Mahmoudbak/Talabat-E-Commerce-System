using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services.Content
{
    public interface IResponseCacheService
    {
        //set
        Task CacheResponseAsync(string key, object Response, TimeSpan timeToLive);


        //get
        Task<string?> GetCachedResponseAsync(string key);

    }
}
    