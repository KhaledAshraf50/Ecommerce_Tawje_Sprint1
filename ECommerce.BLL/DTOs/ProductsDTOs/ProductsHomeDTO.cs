namespace ECommerce_Tawj.DTOs.ProductsDTOs
{
    public class ProductsHomeDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int DiscountPercentage { get; set; }
        public double AverageRating { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string MainImage { get; set; } = string.Empty;

        public bool IsFavorite { get; set; }
    }
}
