using GraduationProjectApi.Repositories.IService.Appointments;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.DTOs;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateAppointmentController : ControllerBase
    {
        private readonly ICreateAppointmentService _appointmentService;

        public CreateAppointmentController(ICreateAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult> PostAppointment(AppointmentDto appointmentDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { StatusCode = 400, MessageEn = "User ID not found", MessageAr = "معرف المستخدم غير موجود" });
            }

            var hasScheduled = await _appointmentService.CheckScheduledAppointmentExistsAsync(userId);
            if (hasScheduled)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, new { StatusCode = 406, MessageEn = "Existing scheduled appointment", MessageAr = "تم العثور على موعد مجدول موجود" });
            }

            var countStudentAppointments = await _appointmentService.CountCompletedAppointmentsByUserAndRoleAsync(userId, "Student");
            if (countStudentAppointments >= 3)
            {
                return StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, new { StatusCode = 407, MessageEn = "Free plan ended", MessageAr = "الخطة المجانية قد انتهت" });
            }

            var appointment = await _appointmentService.CreateAppointmentAsync(appointmentDto, userId);
            return Ok(new { StatusCode = 200, MessageEn = "Appointment created", MessageAr = "تم إنشاء الموعد" });
        }
    }
}
