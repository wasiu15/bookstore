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

        // Retrieve the shopping cart for a given user from Redis cache.
        public async Task<ShoppingCart> GetBasket(string userName)
        {
            // Attempt to get the basket from Redis cache using the username as the key.
            var basket = await _redisCache.GetStringAsync(userName);

            // If the basket is not found or is empty, return null.
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }

            // Deserialize the JSON data retrieved from Redis into a ShoppingCart object and return it.
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        // Update the shopping cart in the Redis cache.
        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            // Check if there is an existing basket in the Redis cache for the user.
            var existingBasket = await _redisCache.GetStringAsync(basket.UserName);

            // If an existing basket is found, merge the items from the updated basket into it.
            if (!string.IsNullOrEmpty(existingBasket))
            {
                ShoppingCart updatedBasket = JsonConvert.DeserializeObject<ShoppingCart>(existingBasket);

                // If the updated basket contains items, add them to the existing basket.
                if (updatedBasket.Items.Count > 0)
                {
                    foreach (var item in basket.Items)
                    {
                        updatedBasket.Items.Add(item);
                    }
                    // Update the basket to be the merged basket.
                    basket = updatedBasket;
                }
            }

            // Store the updated basket in the Redis cache as a JSON string.
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            // Return the updated shopping cart.
            return await GetBasket(basket.UserName);
        }

        // Delete the shopping cart of a user from the Redis cache.
        public async Task DeleteBasket(string userName)
        {
            // Remove the shopping cart associated with the provided username from the Redis cache.
            await _redisCache.RemoveAsync(userName);
        }

    }
}
