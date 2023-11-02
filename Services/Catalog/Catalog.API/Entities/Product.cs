using Catalog.API.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Entities
{
    public class Product
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string CoverImageUrl { get; set; }
        public decimal Price { get; set; }
        public string AboutAuthor { get; set; }
        public int AvailableQuantity { get; set; }
        public DateTime PublicationDate { get; set; }


        public static explicit operator Product(CreateProductDto productDto)
        {
            ValidateDto(productDto);

            return MapDtoToProduct(productDto, Guid.NewGuid().ToString());
        }

        public static explicit operator Product(UpdateProductDto productDto)
        {
            ValidateDto(productDto);

            return MapDtoToProduct(productDto, productDto.Id);
        }

        private static void ValidateDto(ProductDtoBase productDto)
        {
            if (productDto == null)
                throw new ArgumentNullException(nameof(productDto));

            if (string.IsNullOrEmpty(productDto.Title))
                throw new ArgumentException("Title is required", nameof(productDto.Title));

            if (productDto.Price <= 0)
                throw new ArgumentException("Price must be greater than 0", nameof(productDto.Price));

            if (string.IsNullOrEmpty(productDto.CoverImageUrl))
                throw new ArgumentException("CoverImageUrl is required", nameof(productDto.CoverImageUrl));
        }

        private static Product MapDtoToProduct(ProductDtoBase productDto, string productId)
        {
            return new Product
            {
                Id = productId,
                Title = productDto.Title,
                Genre = productDto.Genre,
                CoverImageUrl = productDto.CoverImageUrl,
                Price = productDto.Price,
                AboutAuthor = productDto.AboutAuthor,
                AvailableQuantity = productDto.AvailableQuantity,
                PublicationDate = DateTime.Now
            };
        }
    }
}