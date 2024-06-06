using System.ComponentModel.DataAnnotations;

namespace SharedClassLibrary.DTOs
{
    public class AppointmentUpdateDto
    {
        public string Status { get; set; }
        public string PatientNotes { get; set; }
        public string DoctorNotes { get; set; }
    }
}
