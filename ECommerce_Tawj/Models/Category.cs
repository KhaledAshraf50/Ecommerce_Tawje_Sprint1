using System.ComponentModel.DataAnnotations;

namespace ECommerce_Tawj.Models
{
    public class Category : IAuditableEntity
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation property for the related Products
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
