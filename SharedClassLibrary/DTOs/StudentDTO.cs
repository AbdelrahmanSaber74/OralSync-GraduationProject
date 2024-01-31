using System.ComponentModel.DataAnnotations;
namespace SharedClassLibrary.DTOs
{
    public class StudentDTO
    {
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        public bool IsMale { get; set; }
        public string PhoneNumber { get; set; }
        public string UniversityName { get; set; }
        public double GPA { get; set; }

        public DateTime BirthDate { get; set; }
        public int AcademicYear { get; set; }




    }
}
