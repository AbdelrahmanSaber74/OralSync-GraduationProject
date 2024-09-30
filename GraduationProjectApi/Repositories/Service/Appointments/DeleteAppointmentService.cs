using GraduationProjectApi.Repositories.IService.Appointments;
using IdentityManagerServerApi.Data;
using System.Threading.Tasks;

namespace GraduationProjectApi.Repositories.Service.Appointments
{
    public class DeleteAppointmentService : IDeleteAppointmentService
    {
        private readonly AppDbContext _context;

        public DeleteAppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {   
                return false; // Appointment not found
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
