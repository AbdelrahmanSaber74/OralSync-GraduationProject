using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.Doctor
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllDoctorsController : ControllerBase
    {
        private readonly IUserAccount _userAccount;
        private readonly AppDbContext _db;

        public GetAllDoctorsController(IUserAccount userAccount, AppDbContext db)
        {
            _userAccount = userAccount ?? throw new ArgumentNullException(nameof(userAccount));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var doctors = await _db.Doctors.ToListAsync();

                if (doctors == null)
                {
                    // Log the error for debugging
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        StatusCode = 500,
                        MessageEn = "Error: Unable to retrieve doctors.",
                        MessageAr = "خطأ: غير قادر على استرداد الأطباء."
                    });
                }

                return Ok(doctors);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    MessageEn = "An error occurred while retrieving doctors.",
                    MessageAr = "حدث خطأ أثناء استرداد الأطباء.",
                    ErrorDetails = ex.Message
                });
            }
        }
    }
}
