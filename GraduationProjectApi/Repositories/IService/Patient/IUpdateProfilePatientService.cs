using GraduationProjectApi.Controllers.Students;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using SharedClassLibrary.DTOs;
using System.Threading.Tasks;

public interface IUpdateProfilePatientService
{ 
    Task<Patient> GetPatientByUserIdAsync(string userId);
    Task<ApplicationUser> GetUserByIdAsync(string userId);
    Task UpdatePatientProfileAsync(PatientDTO patientDTO, string userId);
}
