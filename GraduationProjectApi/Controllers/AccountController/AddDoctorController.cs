using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Migrations;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System.Linq;

namespace IdentityManagerServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDoctorController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AddDoctorController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult Post(bool IsDoctor, DoctorDTO doctorDTO)
        {
            var lastUserId = _db.Users.Where(n => n.Name != "Admin")
                .OrderByDescending(u => u.TimeAddUser)
                .Select(u => u.Id)
                .FirstOrDefault();


            if (lastUserId != null && IsDoctor)
            {
                var newDoctor = new Doctor
                {
                    UserId = lastUserId,
                    FirstName = doctorDTO.FirstName,
                    LastName = doctorDTO.LastName,
                    IsMale = doctorDTO.IsMale,
                    PhoneNumber = doctorDTO.PhoneNumber,
                    Email = doctorDTO.Email,
                    UniversityName = doctorDTO.UniversityName,
                    ClinicAddress = doctorDTO.ClinicAddress,
                    ClinicNumber = doctorDTO.ClinicNumber,
                    GPA = doctorDTO.GPA,
                    GraduationDate = doctorDTO.GraduationDate,
                    BirthDate = doctorDTO.BirthDate,
                    Certificates = doctorDTO.Certificates,
                    InsuranceCompanies = doctorDTO.InsuranceCompanies,
                };

                _db.Doctors.Add(newDoctor);
                _db.SaveChanges();

                return Ok(new { message = "Doctor added successfully." });
            }

            return BadRequest(new { errors = "Invalid request or not a doctor." });
        }
    }
}
