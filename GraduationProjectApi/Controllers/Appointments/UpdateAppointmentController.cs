using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using SharedClassLibrary.DTOs;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateAppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UpdateAppointmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, [FromBody] AppointmentUpdateDto appointmentUpdateDto)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);

                if (appointment == null)
                {
                    return NotFound(new { StatusCode = 404, MessageEn = "Appointment not found.", MessageAr = "الموعد غير موجود." });
                }

                // Update appointment properties
                appointment.Status = appointmentUpdateDto.Status;
                appointment.PatientNotes = appointmentUpdateDto.PatientNotes;
                appointment.DoctorNotes = appointmentUpdateDto.DoctorNotes;
                appointment.TimeAppointment = appointmentUpdateDto.TimeAppointment;
                appointment.DateAppointment = appointmentUpdateDto.DateAppointment;

                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, MessageEn = "Appointment updated successfully.", MessageAr = "تم تحديث الموعد بنجاح." });
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
