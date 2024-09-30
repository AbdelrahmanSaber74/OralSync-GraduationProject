using GraduationProjectApi.Models;
using GraduationProjectApi.Repositories.IService.Appointments;
using IdentityManagerServerApi.Data;
using SharedClassLibrary.DTOs;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectApi.Repositories.Service.Appointments
{
    public class UpdateAppointmentService : IUpdateAppointmentService
    {
        private readonly AppDbContext _context;

        public UpdateAppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateAppointmentAsync(int id, AppointmentUpdateDto appointmentUpdateDto)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return false; // Appointment not found
            }

            // Validate the status value
            var validStatuses = new[] { "Scheduled", "Completed", "Cancelled" };
            if (!validStatuses.Contains(appointmentUpdateDto.Status))
            {
                throw new ArgumentException("Invalid status value.");
            }

            // Update appointment properties
            appointment.Status = appointmentUpdateDto.Status;
            appointment.PatientNotes = appointmentUpdateDto.PatientNotes;
            appointment.DoctorNotes = appointmentUpdateDto.DoctorNotes;
            appointment.TimeAppointment = appointmentUpdateDto.TimeAppointment;
            appointment.DateAppointment = appointmentUpdateDto.DateAppointment;

            await _context.SaveChangesAsync();

            return true; // Update successful
        }
    }
}
