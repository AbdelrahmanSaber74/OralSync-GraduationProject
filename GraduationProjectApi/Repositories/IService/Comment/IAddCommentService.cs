using IdentityManagerServerApi.Models;
using SharedClassLibrary.DTOs;

namespace GraduationProjectApi.Repositories.IService
{
    public interface IAddCommentService
    {
        Comment AddComment(CommentDTO commentDto, string userId, string userName);
       GraduationProjectApi.Models.Post GetPostById(int postId); // Add method to retrieve a post by ID if needed
        string GetUserProfileImage(string userId); // New method for getting the profile image

    }
}
