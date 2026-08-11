namespace ECommerce_Tawj.DTOs.UserDTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "Admin" or "Customer"
        public bool IsLocked { get; set; }
    }
}
