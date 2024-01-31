using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagerServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddStudent : ControllerBase
    {


        [HttpPost]
        public IActionResult Post( bool IsStudent , StudentDTO studentDTO)
        {
            AppDbContext db = new AppDbContext();

            var lastUserId = db.Users.Where(n => n.Name != "Admin")
                                   .OrderByDescending(u => u.TimeAddUser)
                                   .Select(u => u.Id)
                                   .FirstOrDefault();
            if ( lastUserId != null && IsStudent)
            {
                var newStudent = new Student
                {
                    UserId = lastUserId,
                    Name = studentDTO.Name,
                    IsMale = studentDTO.IsMale,
                    PhoneNumber = studentDTO.PhoneNumber,
                    Email = studentDTO.Email,
                    UniversityName = studentDTO.UniversityName,
                    AcademicYear = studentDTO.AcademicYear,
                    GPA = studentDTO.GPA,
                    BirthDate = studentDTO.BirthDate,
                };

                db.Students.Add(newStudent);
                db.SaveChanges();

                return Ok(new { message = "Student added successfully." });
             
            }

            return BadRequest(new { message = "Invalid role." });
        }

       
    }
}
