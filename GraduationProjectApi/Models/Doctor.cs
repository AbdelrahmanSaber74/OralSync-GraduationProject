using IdentityManagerServerApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduationProjectApi.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public bool IsMale { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string UniversityName { get; set; }

        public double? GPA { get; set; }

        public string ClinicNumber { get; set; }

        // List of strings for clinic addresses
        public List<string> ClinicAddresses { get; set; } = new List<string>();

        // List of strings for insurance companies
        public List<string> InsuranceCompanies { get; set; } = new List<string>();

        // List of strings for certificates
        public List<string> Certificates { get; set; } = new List<string>();

        public string GraduationDate { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        public string Governorate { get; set; }
        public string UserId { get; set; }

        // Navigation property for the associated user
        public ApplicationUser User { get; set; }
    }
}
