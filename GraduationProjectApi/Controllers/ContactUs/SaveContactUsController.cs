using System;
using System.Linq;
using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.DTOs;
using SharedClassLibrary.Helper;

namespace GraduationProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveContactUsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public SaveContactUsController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }


        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] ContactUsDTO contactUs)
        {

         
            if (contactUs == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { StatusCode = 400, MessageEn = "Invalid contact form data", MessageAr = "بيانات الرسالة غير صالحة" });

            }

            var newRequest = new ContactUs
            {
                FullName = contactUs.FullName,
                Email = contactUs.Email,
                PhoneNumber = contactUs.PhoneNumber,
                Message = contactUs.Message,
                DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),

            };


            _db.Add(newRequest);
            _db.SaveChanges();

            return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Contact form submitted successfully", MessageAr = "تم حفظ الرسالة بنجاح" });
        }



    }
}
