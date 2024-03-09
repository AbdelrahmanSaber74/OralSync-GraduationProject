using System;
using System.ComponentModel.DataAnnotations;

namespace SharedClassLibrary.DTOs
{
    public class ContactUsDTO
    {
        [StringLength(50, ErrorMessage = "Full name cannot be longer than 50 characters")]
        public string? FullName { get; set; }

        [StringLength(50, ErrorMessage = "Email cannot be longer than 50 characters")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number can only contain digits")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }
}
