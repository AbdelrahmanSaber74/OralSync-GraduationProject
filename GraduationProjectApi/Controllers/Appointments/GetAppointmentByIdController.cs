using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GraduationProjectApi.Repositories.IService.Appointments;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAppointmentByIdController : ControllerBase
    {
        private readonly IGetAppointmentByIdService _getAppointmentByIdService;

        public GetAppointmentByIdController(IGetAppointmentByIdService getAppointmentByIdService)
        {
            _getAppointmentByIdService = getAppointmentByIdService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _getAppointmentByIdService.GetAppointmentByIdAsync(id);

            if (appointment == null)
            {
                return NotFound(new { StatusCode = 404, MessageEn = "Appointment not found.", MessageAr = "الموعد غير موجود." });
            }

            return Ok(appointment);
        }
    }
}
