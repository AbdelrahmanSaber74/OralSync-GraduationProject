using GraduationProjectApi.Models;
using GraduationProjectApi.Repositories.IService.Appointments;
using IdentityManagerServerApi.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectApi.Repositories.Service.Appointments
{
    public class GetWaitingScheduledDoctorAppointmentService : IGetWaitingScheduledDoctorAppointmentService
    {
        private readonly AppDbContext _context;

        public GetWaitingScheduledDoctorAppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetWaitingScheduledAppointmentsAsync(string userId, string hostUrl)
        {
            var appointments = await _context.Appointments
                .Where(m => m.DoctorId == userId && (m.Status == "Waiting" || m.Status == "Scheduled"))
                .Select(m => new
                {
                    m.Id,
                    m.DoctorId,
                    m.PatientId,
                    m.DateCreated,
                    m.TimeCreated,
                    m.DateAppointment,
                    m.TimeAppointment,
                    m.Status,
                    m.Location,
                    m.PatientNotes,
                    m.DoctorNotes,
                    m.PaymentMethod,
                    m.Fee,
                    User = _context.Users.Where(u => u.Id == m.PatientId).Select(u => new
                    {
                        u.Name,
                        ProfileImage = hostUrl + u.ProfileImage,
                        Age = CalculateAge(_context.Patients.Where(p => p.UserId == u.Id).Select(p => p.BirthDate).FirstOrDefault())
                    }).FirstOrDefault()
                })
                .ToListAsync();

            return appointments;
        }

        private static int CalculateAge(string birthDateStr)
        {
            if (string.IsNullOrEmpty(birthDateStr))
                return 0; // Return 0 or handle accordingly if birth date is null or empty

            DateTime birthDate;
            if (!DateTime.TryParseExact(birthDateStr, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out birthDate))
            {
                // If parsing fails, handle the error
                throw new ArgumentException("Invalid birth date format.");
            }

            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
                age--;

            return age;
        }
    }
}
