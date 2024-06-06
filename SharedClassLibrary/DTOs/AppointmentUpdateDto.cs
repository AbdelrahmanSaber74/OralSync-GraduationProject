using System.ComponentModel.DataAnnotations;

namespace SharedClassLibrary.DTOs
{
    public class AppointmentUpdateDto
    {
        public string Status { get; set; }
        public string PatientNotes { get; set; }
        public string DoctorNotes { get; set; }

        [StringLength(50)] // Limit the message to 50 characters
        public string DateAppointment { get; set; }

        [StringLength(50)] // Limit the message to 50 characters
        public string TimeAppointment { get; set; }
    }
}
