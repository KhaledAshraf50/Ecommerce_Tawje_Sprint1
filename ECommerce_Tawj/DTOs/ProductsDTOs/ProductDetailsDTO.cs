namespace ECommerce_Tawj.DTOs.ProductsDTOs
{
    public class ProductDetailsDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int DiscountPercentage { get; set; }

        public decimal FinalPrice => (DiscountPercentage > 0 && DiscountPercentage <= 100)
            ? Price - (Price * DiscountPercentage / 100m)
            : Price;

        public string Brand { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public List<ProductImageDTO> Images { get; set; } = new();
    }
}