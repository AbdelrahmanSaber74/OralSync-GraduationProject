using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        
      

        [HttpGet]
        public async Task<IActionResult> Get()
        {


            return Ok("Done");
        }




    }
}
