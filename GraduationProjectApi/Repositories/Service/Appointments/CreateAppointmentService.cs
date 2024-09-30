using GraduationProjectApi.Repositories.IService.Appointments;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.DTOs;
using SharedClassLibrary.Helper;


namespace GraduationProjectApi.Repositories.Service.Appointments
{
    public class CreateAppointmentService : ICreateAppointmentService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateAppointmentService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Appointment> CreateAppointmentAsync(AppointmentDto appointmentDto, string userId)
        {
            var appointment = new Appointment
            {
                DoctorId = appointmentDto.DoctorId,
                PatientId = userId,
                Status = "Waiting",
                Location = appointmentDto.Location,
                DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
                DateAppointment = appointmentDto.DateAppointment,
                TimeAppointment = appointmentDto.TimeAppointment,
                PatientNotes = appointmentDto.PatientNotes,
                DoctorNotes = appointmentDto.DoctorNotes,
                PaymentMethod = appointmentDto.PaymentMethod,
                Fee = appointmentDto.Fee,
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }

        public async Task<int> CountCompletedAppointmentsByUserAndRoleAsync(string userId, string role)
        {
            var appointments = await _context.Appointments
                .Where(a => a.PatientId == userId && a.Status == "Completed")
                .ToListAsync();

            int count = 0;

            foreach (var appointt in appointments)
            {
                var doctor = await _userManager.FindByIdAsync(appointt.DoctorId);
                if (doctor != null)
                {
                    var roles = await _userManager.GetRolesAsync(doctor);
                    if (roles.Contains(role))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public async Task<bool> CheckScheduledAppointmentExistsAsync(string userId)
        {
            return await _context.Appointments
                .Where(m => m.PatientId == userId && m.Status == "Scheduled")
                .AnyAsync();
        }
    }
}
