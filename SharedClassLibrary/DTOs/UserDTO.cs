using System.ComponentModel.DataAnnotations;

namespace SharedClassLibrary.DTOs
{
    public class UserDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required.")]
        public bool IsMale { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please specify if user is a doctor.")]
        public bool IsDoctor { get; set; }

        [Required(ErrorMessage = "Please specify if user is a student.")]
        public bool IsStudent { get; set; }

        [Required(ErrorMessage = "Please specify if user is a patient.")]
        public bool IsPatient { get; set; }
    }
}
