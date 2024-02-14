using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SharedClassLibrary.DTOs
{
    public class StudentDTO
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required.")]
        public bool IsMale { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        public string? UniversityName { get; set; }

        public List<string>? UniversityAddress { get; set; }

        [Range(2, 5, ErrorMessage = "GPA must be between 2 and 5.")]
        public double? GPA { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        public string BirthDate { get; set; }

        public int? AcademicYear { get; set; }
    }
}
