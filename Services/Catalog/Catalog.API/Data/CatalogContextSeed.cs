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
                    Title = "To Kill a Mockingbird",
                    Genre = "Fiction",
                    CoverImageUrl = "mockingbird-cover.png",
                    AboutAuthor = "Harper Lee",
                    Price = 950.00M,
                    AvailableQuantity = 50,
                    PublicationDate = DateTime.UtcNow
                },
                new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "The Great Gatsby",
                    Genre = "Non-Fiction",
                    CoverImageUrl = "gatsby-cover.png",
                    AboutAuthor = "F. Scott Fitzgerald",
                    Price = 320.00M,
                    AvailableQuantity = 30,
                    PublicationDate = DateTime.UtcNow
                }
            };
        }
    }
}
