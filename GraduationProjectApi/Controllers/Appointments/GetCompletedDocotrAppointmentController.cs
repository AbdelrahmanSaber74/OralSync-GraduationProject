using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraduationProjectApi.Repositories.IService.Appointments;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCompletedDoctorAppointmentController : ControllerBase
    {
        private readonly IGetCompletedDoctorAppointmentService _completedDoctorAppointmentService;

        public GetCompletedDoctorAppointmentController(IGetCompletedDoctorAppointmentService completedDoctorAppointmentService)
        {
            _completedDoctorAppointmentService = completedDoctorAppointmentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string hostUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            if (userId == null)
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    MessageEn = "Unauthorized access.",
                    MessageAr = "الوصول غير المصرح به."
                });
            }

            var appointments = await _completedDoctorAppointmentService.GetCompletedAppointmentsAsync(userId, hostUrl);

            if (appointments == null || !appointments.Any())
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    MessageEn = "Completed appointments not found.",
                    MessageAr = "المواعيد المكتملة غير موجودة."
                });
            }

            return Ok(appointments);
        }
    }
}
