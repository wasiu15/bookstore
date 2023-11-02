namespace Catalog.API.Dtos
{
    public class ProductDtoBase
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string CoverImageUrl { get; set; }
        public decimal Price { get; set; }
        public string AboutAuthor { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
