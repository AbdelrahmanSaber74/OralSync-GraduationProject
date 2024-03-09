using System.ComponentModel.DataAnnotations;

namespace SharedClassLibrary.DTOs
{
    public class RatingDTO
    {
        // ID of the user who is being rated (receiver)
        [Required(ErrorMessage = "RatedUserId is required")]
        public string RatedUserId { get; set; }

        [Range(1, 5, ErrorMessage = "Value must be between 1 and 5")]
        public int Value { get; set; } // Assuming the rating scale is from 1 to 5

        public string Comment { get; set; }
    }
}
