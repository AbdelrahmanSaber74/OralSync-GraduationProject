using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteAppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeleteAppointmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound(new { StatusCode = 404, MessageEn = "Appointment not found.", MessageAr = "الموعد غير موجود." });
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, MessageEn = "Appointment deleted successfully.", MessageAr = "تم حذف الموعد بنجاح." });
        }
    }
}
