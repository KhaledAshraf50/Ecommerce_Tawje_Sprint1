namespace ECommerce_Tawj.Models
{
    public interface IAuditableEntity
    {
        bool IsDeleted { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}
