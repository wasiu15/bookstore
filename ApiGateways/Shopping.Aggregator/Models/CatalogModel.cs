namespace Shopping.Aggregator.Models
{
    public class CatalogModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string CoverImageUrl { get; set; }
        public decimal Price { get; set; }
        public string AboutAuthor { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
