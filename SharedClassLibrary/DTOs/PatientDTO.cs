using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SharedClassLibrary.DTOs
{
    public class PatientDTO
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        public bool IsMale { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        public List<string>? Address { get; set; }

        public string? InsuranceCompany { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        public string BirthDate { get; set; }
    }
}
