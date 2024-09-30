using GraduationProjectApi.Models;
using GraduationProjectApi.Repositories.IService.Appointments;
using IdentityManagerServerApi.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectApi.Repositories.Service.Appointments
{
    public class GetWaitingScheduledPatientAppointmentService : IGetWaitingScheduledPatientAppointmentService
    {
        private readonly AppDbContext _context;

        public GetWaitingScheduledPatientAppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetWaitingScheduledAppointmentsAsync(string userId, string hostUrl)
        {
            var appointments = await _context.Appointments
                .Where(m => m.PatientId == userId && (m.Status == "Waiting" || m.Status == "Scheduled"))
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
