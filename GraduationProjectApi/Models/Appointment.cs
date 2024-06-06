using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GraduationProjectApi.Models;

namespace IdentityManagerServerApi.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }

        [StringLength(50)] // Limit the message to 50 characters
        public string DateCreated { get; set; }

        [StringLength(50)] // Limit the message to 50 characters
        public string TimeCreated { get; set; }
        public string Status { get; set; } // e.g., Scheduled, Completed, Cancelled
        public string Location { get; set; } // Location of the appointment
        public string PatientNotes { get; set; } // Notes added by the patient
        public string DoctorNotes { get; set; } // Notes added by the doctor
        public string PaymentMethod { get; set; } // e.g., Cash, Credit Card, Insurance
        public decimal Fee { get; set; } // Fee for the appointment



    }
}
