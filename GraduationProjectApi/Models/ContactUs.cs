using System.ComponentModel.DataAnnotations;

namespace GraduationProjectApi.Models
{
    public class ContactUs
    {
        public int ContactUsId { get; set; }

        [StringLength(200)] // Limit the full name to 50 characters
        public string? FullName { get; set; }

        [StringLength(50)] // Limit the email address to 50 characters
        public string? Email { get; set; }

        [StringLength(20)] // Limit the phone number to 20 characters
        public string? PhoneNumber { get; set; }

        public string Message { get; set; }

        [StringLength(50)] // Limit the message to 50 characters
        public string DateCreated { get; set; }

        [StringLength(50)] // Limit the message to 50 characters
        public string TimeCreated { get; set; }
    }
}
