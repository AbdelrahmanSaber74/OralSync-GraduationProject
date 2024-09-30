using GraduationProjectApi.Repositories.IService;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using SharedClassLibrary.DTOs;
using SharedClassLibrary.Helper;

namespace GraduationProjectApi.Repositories.Service
{
    public class AddCommentService : IAddCommentService
    {
        private readonly AppDbContext _db;

        public AddCommentService(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Comment AddComment(CommentDTO commentDto, string userId, string userName)
        {
            var comment = new Comment
            {
                UserId = userId,
                Name = userName,
                PostId = commentDto.PostId,
                Content = commentDto.Content,
                Title = commentDto.Title,
                DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                TimeCreated = DateTimeHelper.FormatTime(DateTime.Now)
            };

            _db.Comments.Add(comment);
            _db.SaveChanges();
            return comment; // Return the added comment
        }

        public GraduationProjectApi.Models.Post GetPostById(int postId)
        {
            return _db.Posts.FirstOrDefault(m => m.PostId == postId);
        }


        // AddCommentRepository.cs
        public string GetUserProfileImage(string userId)
        {
            return _db.Users.Where(m => m.Id == userId).Select(m => m.ProfileImage).FirstOrDefault();
        }

    }
}
