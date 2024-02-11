//using GraduationProjectApi.Models;
//using IdentityManagerServerApi.Data;
//using IdentityManagerServerApi.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using System.Security.Claims;
//using post = GraduationProjectApi.Models.Post;

//namespace GraduationProjectApi.Controllers.Post
//{
//    [Route("api/[controller]")]
//    [ApiController]

//    [Authorize(Roles="Student,Doctor,Admin")]
//    public class UpdatePostController : ControllerBase
//    {

//        private readonly AppDbContext _db;

//        public UpdatePostController(AppDbContext db)
//        {
//            _db = db;

//        }

//        [HttpGet]
//        public async Task<IActionResult> Get()
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

//            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
//                return BadRequest(new { StatusCode = 400, Message = "User ID or Role not found in claims." });



//            var reuslt = new post
//            {

//                Title = "tst";



//            }

//        }
//    }
