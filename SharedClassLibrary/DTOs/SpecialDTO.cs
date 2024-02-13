using System.ComponentModel.DataAnnotations;


namespace SharedClassLibrary.DTOs
{
    public class SpecialDTO
    {


        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;


        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool IsMale { get; set; }

        public string PhoneNumber { get; set; }



        public bool IsDoctor { get; set; }
        public bool IsStudent { get; set; }
        public bool IsPatient { get; set; }





        /// ///////////////////// Doctor
        public string FirstName { get; set; } 
        public string LastName { get; set; } 

        public string? UniversityName { get; set; }
        public List<string>? ClinicAddress { get; set; }
        public string? ClinicNumber { get; set; }
        public List<string> ?InsuranceCompanies { get; set; }
        public List<string>? Certificates { get; set; }
        public double? GPA { get; set; }
        public string BirthDate { get; set; }
        public string? GraduationDate { get; set; }




        ////////////////////////


        ////////////////////////
        //Patient 


       
        public List<string>? Address { get; set; }
        public string? InsuranceCompany { get; set; }




        ////////////////////////
        //Student 

        public List<string>? UniversitAddress { get; set; }
        public int? AcademicYear { get; set; }

    }
}
