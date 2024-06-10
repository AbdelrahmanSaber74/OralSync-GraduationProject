using System.ComponentModel.DataAnnotations;

namespace SharedClassLibrary.DTOs
{
    public class AppointmentDto
    {


        public string DoctorId { get; set; }
        public string Location { get; set; } // Location of the appointment


        [StringLength(50)] // Limit the message to 50 characters
        public string DateAppointment { get; set; }

        [StringLength(50)] // Limit the message to 50 characters
        public string TimeAppointment { get; set; }

        public string PatientNotes { get; set; } // Notes added by the patient
        public string DoctorNotes { get; set; } // Notes added by the doctor
        public string PaymentMethod { get; set; } // e.g., Cash, Credit Card, Insurance
        public decimal Fee { get; set; } // Fee for the appointment



    }
}
