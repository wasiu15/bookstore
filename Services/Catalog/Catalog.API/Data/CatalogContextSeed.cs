using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "IPhone X",
                    Genre = "Genre",
                    CoverImageUrl = "product-1.png",
                    AboutAuthor = "Author1",
                    Price = 950.00M,
                    AvailableQuantity = 5,
                    PublicationDate = DateTime.UtcNow
                },
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Samsung 10",
                    Genre = "Genre2",
                    CoverImageUrl = "product-2.png",
                    AboutAuthor = "Author2",
                    Price = 320.00M,
                    AvailableQuantity = 20,
                    PublicationDate = DateTime.UtcNow
                }
            };
        }
    }
}
