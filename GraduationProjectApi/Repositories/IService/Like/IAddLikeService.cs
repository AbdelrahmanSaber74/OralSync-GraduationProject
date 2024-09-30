using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.DTOs;
using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public interface IAddLikeService
    {
        Task<IActionResult> AddOrRemoveLikeAsync(int postId, string userId);
    }
}
