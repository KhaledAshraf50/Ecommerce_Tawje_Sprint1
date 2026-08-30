namespace ECommerce_Tawj.DTOs.UserDTOs
{
    public class UserListDTO
    {
        public IEnumerable<UserDTO> Users { get; set; } = new List<UserDTO>();
        public string SearchTerm { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
