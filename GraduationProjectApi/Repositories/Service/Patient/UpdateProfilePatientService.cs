using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using SharedClassLibrary.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UpdateProfilePatientService : IUpdateProfilePatientService
{
    private readonly AppDbContext _db;

    public UpdateProfilePatientService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Patient> GetPatientByUserIdAsync(string userId)
    {
        return await _db.Patients.FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string userId)
    {
        return await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task UpdatePatientProfileAsync(PatientDTO patientDTO, string userId)
    {
        var patient = await GetPatientByUserIdAsync(userId);
        var userPatient = await GetUserByIdAsync(userId);

        if (patient == null || userPatient == null)
        {
            throw new KeyNotFoundException("Patient or User not found.");
        }

        // Update Patient entity
        patient.FirstName = patientDTO.FirstName;
        patient.LastName = patientDTO.LastName;
        patient.IsMale = patientDTO.IsMale;
        patient.Email = patientDTO.Email;
        patient.PhoneNumber = patientDTO.PhoneNumber;
        patient.Address = patientDTO.Address;
        patient.InsuranceCompany = patientDTO.InsuranceCompany;
        patient.BirthDate = patientDTO.BirthDate;
        patient.Governorate = patientDTO.Governorate;

        // Update User entity
        userPatient.Name = $"{patientDTO.FirstName}_{patientDTO.LastName}";
        userPatient.UserName = patientDTO.Email;
        userPatient.NormalizedUserName = patientDTO.Email.ToUpper();
        userPatient.Email = patientDTO.Email;
        userPatient.NormalizedEmail = patientDTO.Email.ToUpper();
        userPatient.PhoneNumber = patientDTO.PhoneNumber;

        _db.Update(patient);
        _db.Update(userPatient);
        await _db.SaveChangesAsync();
    }
}
