using System.ComponentModel.DataAnnotations;
namespace SharedClassLibrary.DTOs
{
    public class PatientDTO
    {
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        public bool IsMale { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string InsuranceCompany { get; set; }
        public DateTime BirthDate { get; set; }


    }
}
