using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SharedClassLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using GraduationProjectApi.Repositories.IService.Appointments;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateAppointmentController : ControllerBase
    {
        private readonly IUpdateAppointmentService _updateAppointmentService;

        public UpdateAppointmentController(IUpdateAppointmentService updateAppointmentService)
        {
            _updateAppointmentService = updateAppointmentService;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAppointment(int id, [FromBody] AppointmentUpdateDto appointmentUpdateDto)
        {
            try
            {
                var isUpdated = await _updateAppointmentService.UpdateAppointmentAsync(id, appointmentUpdateDto);

                if (!isUpdated)
                {
                    return NotFound(new { StatusCode = 404, MessageEn = "Appointment not found.", MessageAr = "الموعد غير موجود." });
                }

                return Ok(new { StatusCode = 200, MessageEn = "Appointment updated successfully.", MessageAr = "تم تحديث الموعد بنجاح." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { StatusCode = 400, MessageEn = ex.Message, MessageAr = "قيمة الحالة غير صالحة." });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = "Concurrency exception occurred while updating appointment.", MessageAr = "حدث استثناء تنافرية أثناء تحديث الموعد." });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = "An error occurred while updating appointment.", MessageAr = "حدث خطأ أثناء تحديث الموعد." });
            }
        }
    }
}
