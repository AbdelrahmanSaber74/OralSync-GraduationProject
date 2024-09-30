// GraduationProjectApi/Repositories/Service/Appointments/GetCompletedPatientAppointmentByUserIdService.cs
using GraduationProjectApi.Repositories.IService.Appointments;
using IdentityManagerServerApi.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectApi.Repositories.Service.Appointments
{
    public class GetCompletedPatientAppointmentByUserIdService : IGetCompletedPatientAppointmentByUserIdService
    {
        private readonly AppDbContext _context;

        public GetCompletedPatientAppointmentByUserIdService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetCompletedAppointmentsAsync(string userId, string hostUrl)
        {
            var appointments = await _context.Appointments
                .Where(m => m.PatientId == userId && m.Status == "Completed")
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
                    m.isRating,
                    User = _context.Users.Where(u => u.Id == m.DoctorId).Select(u => new
                    {
                        u.Name,
                        ProfileImage = hostUrl + u.ProfileImage
                    }).FirstOrDefault()
                })
                .ToListAsync();

            return appointments;
        }
    }
}
