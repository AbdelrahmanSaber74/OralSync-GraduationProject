using System.ComponentModel.DataAnnotations;


namespace SharedClassLibrary.DTOs
{
    public class DoctorDTO
    {

        public string FirstName { get; set; } 
        public string LastName { get; set; } 

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public bool IsMale { get; set; }
        public string PhoneNumber { get; set; }
        public string? UniversityName { get; set; }
        public List<string>? ClinicAddress { get; set; }
        public string? ClinicNumber { get; set; }
        public List<string> ?InsuranceCompanies { get; set; }
        public List<string>? Certificates { get; set; }
        public double? GPA { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? GraduationDate { get; set; }


      

    }
}
