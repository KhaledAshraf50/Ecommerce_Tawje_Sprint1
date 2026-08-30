namespace ECommerce_Tawj.Models
{
    public class Review : IAuditableEntity
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public bool IsDeleted { get; set ; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
