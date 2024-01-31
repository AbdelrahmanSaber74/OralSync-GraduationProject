using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Migrations;
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
    public class AddDoctor : ControllerBase
    {

        [HttpPost]
        public IActionResult Post(bool IsDoctor, DoctorDTO doctorDTO)
        {
            AppDbContext db = new AppDbContext();

            var lastUserId = db.Users.Where(n => n.Name != "Admin")
                .OrderByDescending(u => u.TimeAddUser)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (lastUserId != null && IsDoctor)
                {
                    var newDoctor = new Doctor
                    {
                        UserId = lastUserId,
                        Name = doctorDTO.Name,
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

                    db.Doctors.Add(newDoctor);
                    db.SaveChanges();

                    return Ok(new { message = "Doctor added successfully." });
                }
            }
            else
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
            }

            return BadRequest(new { message = "Invalid role." });
        }
    }
}


//AddCharacters