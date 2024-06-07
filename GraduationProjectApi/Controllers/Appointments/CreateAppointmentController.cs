﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using SharedClassLibrary.DTOs;
using System;
using System.Security.Claims;
using SharedClassLibrary.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateAppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateAppointmentController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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




                ///////////////////////////////////////  Start Check Role Stduent Or Doctor ///////////////
                var doctorOrStudent = await _userManager.FindByIdAsync(appointmentDto.DoctorId);
                if (doctorOrStudent == null)
                {
                    return StatusCode(StatusCodes.Status402PaymentRequired, new { StatusCode = 402, MessageEn = "Doctor or student not found.", MessageAr = "لم يتم العثور على الطبيب أو الطالب." });
                }

                var roles = await _userManager.GetRolesAsync(doctorOrStudent);
                var doctorOrStudentRole = roles.FirstOrDefault();

                if (string.IsNullOrEmpty(doctorOrStudentRole))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { StatusCode = 403, MessageEn = "Doctor or student role not found.", MessageAr = "لم يتم العثور على دور الطبيب أو الطالب." });
                }

                /////////////////////////////////////// End  Check Role Stduent Or Doctor ///////////////

                    
                if (doctorOrStudentRole == "Student")
                {

                   var countScheduledAppointments= await _context.Appointments
                    .Where(a => a.PatientId == userId && a.Status == "Completed")
                    .CountAsync();

                        if (countScheduledAppointments > 3)
                        {
                        return StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, new { StatusCode = 407, MessageEn = "The patient's free plan has already ended, please consult a doctor.", MessageAr = "الخطة المجانية للمريض قد انتهت بالفعل يرجى مراجعة طبيب." });
                         }

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
