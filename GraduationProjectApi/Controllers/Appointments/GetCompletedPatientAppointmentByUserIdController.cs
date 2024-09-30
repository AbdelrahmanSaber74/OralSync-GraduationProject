using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GraduationProjectApi.Repositories.IService.Appointments;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCompletedPatientAppointmentByUserIdController : ControllerBase
    {
        private readonly IGetCompletedPatientAppointmentByUserIdService _completedPatientAppointmentService;

        public GetCompletedPatientAppointmentByUserIdController(IGetCompletedPatientAppointmentByUserIdService completedPatientAppointmentService)
        {
            _completedPatientAppointmentService = completedPatientAppointmentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> Get(string userId)
        {
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

            var appointments = await _completedPatientAppointmentService.GetCompletedAppointmentsAsync(userId, hostUrl);

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
