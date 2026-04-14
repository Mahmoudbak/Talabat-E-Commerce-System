using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entity.Basket;
using Talabat.Core.Repository.contrent;

namespace Talabat.Repsotiory.BasketRepository
{
    public class BasketRepositoy : IBasketRepository
    {
        private readonly  IDatabase _database;
        public BasketRepositoy(IConnectionMultiplexer redis)
        {
            _database=redis.GetDatabase();
        }
        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var basket=await _database.StringGetAsync(BasketId);
            return basket.IsNullOrEmpty? null : JsonSerializer.Deserialize<CustomerBasket?>(basket);
        }
        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var CreatedOrUpdated = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if (!CreatedOrUpdated) return null;

            return await GetBasketAsync(basket.Id);

        }
        public Task<bool> DeleteBasketAsync(string BasketId)
        {
            return _database.KeyDeleteAsync(BasketId);
        }


    }
}
