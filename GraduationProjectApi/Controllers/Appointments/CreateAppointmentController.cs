using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using SharedClassLibrary.DTOs;
using System;
using System.Security.Claims;
using SharedClassLibrary.Helper;
using Microsoft.AspNetCore.Authorization;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateAppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CreateAppointmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<Appointment>> PostAppointment(AppointmentDto appointmentDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {

                    return BadRequest(new { StatusCode = 400, MessageEn = "User ID not found in claims.", MessageAr = "معرف المستخدم غير موجود في البيانات." });
                }


                var checkScheduledStatus = _context.Appointments.Where(m => m.PatientId == userId && m.Status == "Scheduled").FirstOrDefault();

                if (checkScheduledStatus != null)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new { StatusCode = 406, MessageEn = "An existing scheduled appointment was found.", MessageAr = "تم العثور على موعد مجدول موجود بالفعل." });
                }


                // Convert the AppointmentDto to an Appointment entity
                var appointment = new Appointment
                {
                    DoctorId = appointmentDto.DoctorId,
                    PatientId = userId,
                    Status = appointmentDto.Status,
                    Location = appointmentDto.Location,
                    DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                    TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
                    DateAppointment = appointmentDto.DateAppointment,
                    TimeAppointment = appointmentDto.TimeAppointment,
                    PatientNotes = appointmentDto.PatientNotes,
                    DoctorNotes = appointmentDto.DoctorNotes,
                    PaymentMethod = appointmentDto.PaymentMethod,
                    Fee = appointmentDto.Fee,
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Appointment created successfully", MessageAr = "تم إنشاء الموعد بنجاح" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = "An error occurred while creating the appointment", MessageAr = "حدث خطأ أثناء إنشاء الموعد" });
            }
        }
    }
}
