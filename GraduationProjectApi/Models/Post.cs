using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations;

namespace GraduationProjectApi.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }


        public string DateCreated { get; set; }
        public string TimeCreated { get; set; }

        public string DateUpdated { get; set; }
        public string TimeUpdated { get; set; }


        [Required]
        public string UserId { get; set; }

        // Navigation property for the user who authored the post
        public ApplicationUser User { get; set; }
    }
}
