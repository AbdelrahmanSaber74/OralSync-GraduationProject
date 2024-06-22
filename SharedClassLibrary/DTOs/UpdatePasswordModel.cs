using System.ComponentModel.DataAnnotations;

namespace SharedClassLibrary.DTOs
{
    public class UpdatePasswordModel
    {

        [Required(ErrorMessage = "New password is required.")]
        public string NewPassword { get; set; }

    }
}
