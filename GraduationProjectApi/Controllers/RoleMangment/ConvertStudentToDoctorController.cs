using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.RoleMangment
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertStudentToDoctorController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;

        public ConvertStudentToDoctorController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(string userId)
        {
            try
            {
                // Find the user by Id
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        MessageEn = "User not found",
                        MessageAr = "المستخدم غير موجود"
                    });
                }

                // Check if user is in the "student" role
                if (await _userManager.IsInRoleAsync(user, "student"))
                {
                    // Remove user from "student" role
                    var removeFromRoleResult = await _userManager.RemoveFromRoleAsync(user, "student");
                    if (!removeFromRoleResult.Succeeded)
                    {
                        return BadRequest(new
                        {
                            StatusCode = 400,
                            MessageEn = "Failed to remove user from 'student' role",
                            MessageAr = "فشل في إزالة المستخدم من دور 'طالب'"
                        });
                    }

                    // Create Doctor entity
                    var studentRecord = await _db.Students.FirstOrDefaultAsync(s => s.UserId == userId);
                    if (studentRecord != null)
                    {
                        var doctor = new Models.Doctor
                        {
                            UserId = userId,
                            FirstName = studentRecord.FirstName,
                            LastName = studentRecord.LastName,
                            IsMale = Convert.ToBoolean(studentRecord.IsMale),
                            PhoneNumber = studentRecord.PhoneNumber,
                            Email = studentRecord.Email,
                            UniversityName = studentRecord.UniversityName,
                            GPA = studentRecord.GPA,
                            ClinicAddresses = new List<string>(), // Example initialization
                            InsuranceCompanies = new List<string>(), // Example initialization
                            Certificates = new List<string>(), // Example initialization
                            GraduationDate = "", // Example initialization
                            ClinicNumber = "", // Example initialization
                            BirthDate = studentRecord.BirthDate,
                            Governorate = studentRecord.Governorate
                        };

                        // Add Doctor record to database
                        _db.Doctors.Add(doctor);
                        await _db.SaveChangesAsync();

                        // Remove user from students table
                        _db.Students.Remove(studentRecord);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            StatusCode = 400,
                            MessageEn = "Failed to find student record",
                            MessageAr = "فشل في العثور على سجل الطالب"
                        });
                    }
                }

                // Check if "doctor" role exists
                if (!await _roleManager.RoleExistsAsync("doctor"))
                {
                    // Create "doctor" role if it doesn't exist
                    var createRoleResult = await _roleManager.CreateAsync(new IdentityRole("doctor"));
                    if (!createRoleResult.Succeeded)
                    {
                        return BadRequest(new
                        {
                            StatusCode = 400,
                            MessageEn = "Failed to create 'doctor' role",
                            MessageAr = "فشل في إنشاء دور 'دكتور'"
                        });
                    }
                }

                // Add user to "doctor" role
                var addToRoleResult = await _userManager.AddToRoleAsync(user, "doctor");
                if (!addToRoleResult.Succeeded)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        MessageEn = "Failed to add user to 'doctor' role",
                        MessageAr = "فشل في إضافة المستخدم إلى دور 'دكتور'"
                    });
                }

                return Ok(new
                {
                    StatusCode = 200,
                    MessageEn = "User role successfully updated to 'doctor' and removed from students table",
                    MessageAr = "تم تحديث دور المستخدم بنجاح إلى 'دكتور' وإزالته من جدول الطلاب"
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex}");

                // Return a generic error response
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    MessageEn = "An error occurred while processing the request",
                    MessageAr = "حدث خطأ أثناء معالجة الطلب"
                });
            }
        }
    }
}
