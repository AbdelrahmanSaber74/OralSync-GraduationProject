using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.DTOs;
using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public interface IUpdateProfileDoctorService
    {
        Task<IActionResult> UpdateProfileAsync(DoctorDTO doctorDTO, string userId);
    }
}
