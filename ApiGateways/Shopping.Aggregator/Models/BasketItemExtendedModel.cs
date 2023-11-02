namespace Shopping.Aggregator.Models
{
    public class BasketItemExtendedModel
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string BookId { get; set; }
        public string BookName { get; set; }

        //Product Related Additional Fields
        public string Genre { get; set; }
        public string AboutAuthor { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
