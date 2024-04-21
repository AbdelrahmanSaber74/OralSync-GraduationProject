using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.Governorate
{
    [Route("api/[controller]")]
    [ApiController]
    public class getGovernorateController : ControllerBase
    {
        private readonly AppDbContext _db;

        public getGovernorateController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        [AllowAnonymous] // Allow anonymous access for now, you can restrict it as needed
        public async Task<IActionResult> Get()
        {
            try
            {
                var governorates = await _db.Governorate.ToListAsync();

                if (governorates == null || governorates.Count == 0)
                {
                    return NotFound(new { StatusCode = 404, MessageEn = "No governorates were found.", MessageAr = "لم يتم العثور على أي محافظات." });

                }
                return Ok(governorates);


            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                // You can also return a custom error response if needed
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,MessageEn = "An error occurred while retrieving governorates.",MessageAr = "حدث خطأ أثناء استرجاع المحافظات."
                });
            }
        }
    }
}
