using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services.Content;

namespace Talabat.Services.OrderService
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;

         
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string key, object Response, TimeSpan timeToLive)
        {
            if (Response is null) return;
            // CamelCase in FrontEnd
            var SerializedOprtion=new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            
            
            var SerializedResponse = JsonSerializer.Serialize(Response,SerializedOprtion);

            await _database.StringSetAsync(key, SerializedResponse, timeToLive);
            
        }


        public async Task<string?> GetCachedResponseAsync(string key)
        {
         var Response=await _database.StringGetAsync(key);
            
         if(Response.IsNullOrEmpty) return null;

         return Response;
        }
    }
}
