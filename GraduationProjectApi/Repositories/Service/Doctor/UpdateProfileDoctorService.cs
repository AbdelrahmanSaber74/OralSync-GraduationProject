using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.DTOs;

namespace GraduationProjectApi.Services
{
    public class UpdateProfileDoctorService : IUpdateProfileDoctorService
    {
        private readonly AppDbContext _db;

        public UpdateProfileDoctorService(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IActionResult> UpdateProfileAsync(DoctorDTO doctorDTO, string userId)
        {
            var doctor = await _db.Doctors.FirstOrDefaultAsync(x => x.UserId == userId);
            var userDoctor = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (doctor == null)
            {
                return new NotFoundObjectResult(new { StatusCode = 404, MessageEn = "Doctor not found.", MessageAr = "الطبيب غير موجود." });
            }

            try
            {
                // Update Doctor entity
                doctor.FirstName = doctorDTO.FirstName;
                doctor.LastName = doctorDTO.LastName;
                doctor.IsMale = doctorDTO.IsMale;
                doctor.PhoneNumber = doctorDTO.PhoneNumber;
                doctor.Email = doctorDTO.Email;
                doctor.UniversityName = doctorDTO.UniversityName;
                doctor.GPA = doctorDTO.GPA;
                doctor.ClinicAddresses = doctorDTO.ClinicAddress;
                doctor.ClinicNumber = doctorDTO.ClinicNumber;
                doctor.InsuranceCompanies = doctorDTO.InsuranceCompanies;
                doctor.Certificates = doctorDTO.Certificates;
                doctor.GraduationDate = doctorDTO.GraduationDate;
                doctor.BirthDate = doctorDTO.BirthDate;
                doctor.Governorate = doctorDTO.Governorate;

                // Update User entity
                userDoctor.Name = $"{doctorDTO.FirstName}_{doctorDTO.LastName}";
                userDoctor.UserName = doctorDTO.Email;
                userDoctor.NormalizedUserName = doctorDTO.Email.ToUpper();
                userDoctor.Email = doctorDTO.Email;
                userDoctor.NormalizedEmail = doctorDTO.Email.ToUpper();
                userDoctor.PhoneNumber = doctorDTO.PhoneNumber;

                _db.Update(doctor);
                _db.Update(userDoctor); // Make sure to update the userDoctor entity as well
                await _db.SaveChangesAsync();

                return new OkObjectResult(new { StatusCode = 200, MessageEn = "Doctor profile updated successfully.", MessageAr = "تم تحديث ملف الطبيب بنجاح." });
            }
            catch (DbUpdateException ex)
            {
                // Log the exception if needed
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
