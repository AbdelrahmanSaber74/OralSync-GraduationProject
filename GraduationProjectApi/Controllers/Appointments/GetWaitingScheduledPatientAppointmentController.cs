using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProjectApi.Repositories.IService.Appointments;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetWaitingScheduledPatientAppointmentController : ControllerBase
    {
        private readonly IGetWaitingScheduledPatientAppointmentService _waitingScheduledPatientAppointmentService;

        public GetWaitingScheduledPatientAppointmentController(IGetWaitingScheduledPatientAppointmentService waitingScheduledPatientAppointmentService)
        {
            _waitingScheduledPatientAppointmentService = waitingScheduledPatientAppointmentService;
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

            var appointments = await _waitingScheduledPatientAppointmentService.GetWaitingScheduledAppointmentsAsync(userId, hostUrl);

            if (appointments == null || !appointments.Any())
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    MessageEn = "No waiting or scheduled appointments found for this patient.",
                    MessageAr = "لا توجد مواعيد في انتظار أو مجدولة لهذا المريض."
                });
            }

            return Ok(appointments);
        }
    }
}
