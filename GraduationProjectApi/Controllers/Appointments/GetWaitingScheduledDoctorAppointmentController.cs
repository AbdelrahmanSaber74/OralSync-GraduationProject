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
    public class GetWaitingScheduledDoctorAppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GetWaitingScheduledDoctorAppointmentController(AppDbContext context)
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
                        ProfileImage = hosturl + u.ProfileImage,
                        Age = CalculateAge(_context.Patients.Where(p => p.UserId == u.Id).Select(p => p.BirthDate).FirstOrDefault())

                    }).FirstOrDefault()
                })
                .ToListAsync();

            if (appointments == null || !appointments.Any())
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    MessageEn = "No waiting or scheduled appointments found for this doctor.",
                    MessageAr = "لا توجد مواعيد في انتظار أو مجدولة لهذا الطبيب."
                });
            }


            return Ok(appointments);
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