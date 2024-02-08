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
    public class AddPatientController : ControllerBase
    {


        [HttpPost]
        public IActionResult Post(bool IsPatient, PatientDTO patientDTO)
        {

            AppDbContext db = new AppDbContext();

            var lastUserId = db.Users.Where(n => n.Name != "Admin")
                         .OrderByDescending(u => u.TimeAddUser)
                         .Select(u => u.Id)
                         .FirstOrDefault();


            if ( lastUserId != null && IsPatient)
            {
                var newPatient = new Patient
                {
                    UserId = lastUserId,
                    FirstName = patientDTO.FirstName,
                    LastName = patientDTO.LastName,
                    IsMale = patientDTO.IsMale,
                    PhoneNumber = patientDTO.PhoneNumber,
                    Email = patientDTO.Email,
                    Address = patientDTO.Address,
                    InsuranceCompany = patientDTO.InsuranceCompany,
                    BirthDate = patientDTO.BirthDate,
                };

                db.Patients.Add(newPatient);
                db.SaveChanges();

                return Ok(new { message = "Patient added successfully." });
            }



            return BadRequest(new { message = "Invalid role." });
        }

       
    }
}
