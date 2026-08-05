namespace ECommerce_Tawj.DTOs.CategoryDTOs
{
    public class CategoryIndexDTO
    {
        public AddCategoryDTO NewCategory { get; set; } = new AddCategoryDTO();
        public IEnumerable<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
    }
}
