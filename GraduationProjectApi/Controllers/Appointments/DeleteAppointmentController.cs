using Microsoft.AspNetCore.Mvc;
using GraduationProjectApi.Repositories.IService.Appointments;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteAppointmentController : ControllerBase
    {
        private readonly IDeleteAppointmentService _deleteAppointmentService;

        public DeleteAppointmentController(IDeleteAppointmentService deleteAppointmentService)
        {
            _deleteAppointmentService = deleteAppointmentService;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await _deleteAppointmentService.DeleteAppointmentAsync(id);
            if (!result)
            {
                return NotFound(new { StatusCode = 404, MessageEn = "Appointment not found.", MessageAr = "الموعد غير موجود." });
            }

            return Ok(new { StatusCode = 200, MessageEn = "Appointment deleted successfully.", MessageAr = "تم حذف الموعد بنجاح." });
        }
    }
}
