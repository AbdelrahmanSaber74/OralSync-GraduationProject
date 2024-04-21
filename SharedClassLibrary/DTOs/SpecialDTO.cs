using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SharedClassLibrary.DTOs
{
    public class SpecialDTO
    {
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

        // Doctor properties
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        public string? UniversityName { get; set; }
        public List<string>? ClinicAddress { get; set; }
        public string? ClinicNumber { get; set; }
        public List<string>? InsuranceCompanies { get; set; }
        public List<string>? Certificates { get; set; }

        [Range(2, 5, ErrorMessage = "GPA must be between 2 and 5.")]
        public double? GPA { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        public string BirthDate { get; set; }

        public string? GraduationDate { get; set; }

        // Patient properties
        public List<string>? Address { get; set; }
        public string? InsuranceCompany { get; set; }


        [Required(ErrorMessage = "Governorate is required.")]
        public string Governorate { get; set; }



        // Student properties
        public List<string>? UniversityAddress { get; set; }


        public int? AcademicYear { get; set; }
    }
}
