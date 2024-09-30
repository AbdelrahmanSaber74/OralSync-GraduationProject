// GraduationProjectApi/Repositories/Service/Appointments/GetAppointmentsService.cs
using GraduationProjectApi.Repositories.IService.Appointments;
using IdentityManagerServerApi.Data;
using GraduationProjectApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IdentityManagerServerApi.Models;

namespace GraduationProjectApi.Repositories.Service.Appointments
{
    public class GetAppointmentsService : IGetAppointmentsService
    {
        private readonly AppDbContext _context;

        public GetAppointmentsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments.ToListAsync();
        }
    }
}
