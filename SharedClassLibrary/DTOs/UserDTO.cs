using System.ComponentModel.DataAnnotations;
namespace SharedClassLibrary.DTOs
{
    public class UserDTO
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


       
      


    }
}

