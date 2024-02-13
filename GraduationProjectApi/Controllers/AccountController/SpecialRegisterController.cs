using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagerServerApi.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialRegisterController : ControllerBase
    {
        private readonly IUserAccount _userAccount;
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public SpecialRegisterController(IUserAccount userAccount, AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _userAccount = userAccount;
            _db = db;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(SpecialDTO specialDTO)
        {

            var response = await _userAccount.CreateAccountSpecial(specialDTO);

            if (!response.Flag)
            {
                return BadRequest(response);
            }

            bool adminAdded = await AddAdminIfNeeded();

            if (adminAdded)
            {
                return Ok(response);
            }

            var lastUserId = await GetLastId();

            if (lastUserId == null)
            {
                return BadRequest(new { StatusCode = 400, MessageEn = "User ID not found.", MessageAr = "لم يتم العثور على معرف المستخدم." });

            }

            if (specialDTO.IsDoctor)
            {
                AddDoctor(specialDTO,  lastUserId);
            }
            else if (specialDTO.IsStudent)
            {
                AddStudent(specialDTO , lastUserId);
            }
            else if (specialDTO.IsPatient)
            {
                AddPatient(specialDTO, lastUserId);
            }

            await _db.SaveChangesAsync();
            return Ok(response);
        }

        private async Task<bool> AddAdminIfNeeded()
        {

            if (_db.Users.Count() == 1)
            {
                var admin = await _userManager.Users.FirstOrDefaultAsync();
                if (admin != null)
                {
                    admin.Name = "Admin";
                    await _userManager.UpdateAsync(admin);
                    await _db.SaveChangesAsync();
                    return true; // Admin added successfully
                }
            }

            return false; // Admin not added
        }


        private async Task<string?> GetLastId()
        {
            return await _db.Users
                .Where(n => n.Name != "Admin")
                .OrderByDescending(u => u.TimeAddUser)
                .Select(u => u.Id)  
                .FirstOrDefaultAsync();
        }

        private void AddDoctor(SpecialDTO specialDTO, string userId)
        {
            var newDoctor = new Doctor
            {
                FirstName = specialDTO.FirstName,
                LastName = specialDTO.LastName,
                IsMale = specialDTO.IsMale,
                PhoneNumber = specialDTO.PhoneNumber,
                Email = specialDTO.Email,
                UniversityName = specialDTO.UniversityName,
                ClinicAddress = specialDTO.ClinicAddress,
                ClinicNumber = specialDTO.ClinicNumber,
                GPA = specialDTO.GPA,
                GraduationDate = specialDTO.GraduationDate,
                BirthDate = specialDTO.BirthDate,
                Certificates = specialDTO.Certificates,
                InsuranceCompanies = specialDTO.InsuranceCompanies,
                UserId = userId

            };
            _db.Doctors.Add(newDoctor);
        }

        private void AddStudent(SpecialDTO specialDTO, string userId)
        {
            var newStudent = new Student
            {
                FirstName = specialDTO.FirstName,
                LastName = specialDTO.LastName,
                IsMale = specialDTO.IsMale,
                PhoneNumber = specialDTO.PhoneNumber,
                Email = specialDTO.Email,
                UniversityName = specialDTO.UniversityName,
                UniversitAddress = specialDTO.UniversitAddress,
                AcademicYear = specialDTO.AcademicYear,
                GPA = specialDTO.GPA,
                BirthDate = specialDTO.BirthDate,
                UserId = userId

            };
            _db.Students.Add(newStudent);
        }

        private void AddPatient(SpecialDTO specialDTO, string userId)
        {
            var newPatient = new Patient
            {
                FirstName = specialDTO.FirstName,
                LastName = specialDTO.LastName,
                IsMale = specialDTO.IsMale,
                PhoneNumber = specialDTO.PhoneNumber,
                Email = specialDTO.Email,
                Address = specialDTO.Address,
                InsuranceCompany = specialDTO.InsuranceCompany,
                BirthDate = specialDTO.BirthDate,
                UserId = userId
            };
            _db.Patients.Add(newPatient);
        }

    }
}
