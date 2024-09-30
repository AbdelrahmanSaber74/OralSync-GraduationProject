// GraduationProjectApi/Repositories/Service/Appointments/GetAppointmentByIdService.cs
using GraduationProjectApi.Repositories.IService.Appointments;
using IdentityManagerServerApi.Data;
using GraduationProjectApi.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IdentityManagerServerApi.Models;

namespace GraduationProjectApi.Repositories.Service.Appointments
{
    public class GetAppointmentByIdService : IGetAppointmentByIdService
    {
        private readonly AppDbContext _context;

        public GetAppointmentByIdService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }
    }
}
