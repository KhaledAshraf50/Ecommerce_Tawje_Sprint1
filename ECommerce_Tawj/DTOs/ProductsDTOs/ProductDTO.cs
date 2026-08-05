namespace ECommerce_Tawj.ViewModels.ProductsVM
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string MainImage { get; set; } = string.Empty;

    }
}
