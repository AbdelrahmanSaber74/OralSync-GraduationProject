using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetDoctorAppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GetDoctorAppointmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            if (userId == null)
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    MessageEn = "Unauthorized access.",
                    MessageAr = "الوصول غير المصرح به."
                });
            }

            var appointments = await _context.Appointments
                .Where(m => m.DoctorId == userId)
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
                        ProfileImage = hosturl + u.ProfileImage
                    }).FirstOrDefault()
                })
                .ToListAsync();

            if (appointments == null || !appointments.Any())
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    MessageEn = "Appointments not found.",
                    MessageAr = "المواعيد غير موجودة."
                });
            }

            return Ok(appointments);
        }
    }
}
