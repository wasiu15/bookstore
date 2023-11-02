using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if(string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            var existingBasket = await _redisCache.GetStringAsync(basket.UserName);
            if (!string.IsNullOrEmpty(existingBasket))
            {
                ShoppingCart updatedBasket = JsonConvert.DeserializeObject<ShoppingCart>(existingBasket);
                if(updatedBasket.Items.Count > 0)
                {
                    foreach (var item in basket.Items)
                    {
                        updatedBasket.Items.Add(item);
                    }
                    basket = updatedBasket;
                }
            }

            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
    }
}
