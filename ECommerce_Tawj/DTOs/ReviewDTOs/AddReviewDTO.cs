using System.ComponentModel.DataAnnotations;

namespace ECommerce_Tawj.DTOs.ReviewDTOs
{
    public class AddReviewDTO
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage ="Rating Is Required")]
        [Range(1,5,ErrorMessage ="Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Comment Is Required")]
        [StringLength(1000,ErrorMessage ="Comment Cannot exceed 1000 characters")]
        public string Comment { get; set; }
    }
}
