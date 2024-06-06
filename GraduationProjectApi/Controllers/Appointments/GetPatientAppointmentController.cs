using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetPatientAppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GetPatientAppointmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Appointment>> Get()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var appointment = _context.Appointments.Where(m=>m.PatientId == userId).ToList();

            if (appointment == null)
            {
                return NotFound(new { StatusCode = 404, MessageEn = "Appointment not found.", MessageAr = "الموعد غير موجود." });
            }

            return Ok(appointment);

        }
    }
}
