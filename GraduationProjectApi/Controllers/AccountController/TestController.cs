using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.DTOs;

namespace IdentityManagerServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {


        [HttpGet]
        public IActionResult get()
        {
            AppDbContext db = new AppDbContext();



            var result = db.Users.ToList();

            return Ok(result);


        }

       
    }
}
