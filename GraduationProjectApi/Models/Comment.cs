using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GraduationProjectApi.Models;

namespace IdentityManagerServerApi.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Title { get; set; }

        public string DateCreated { get; set; }
        public string TimeCreated { get; set; }

        public string DateUpdated { get; set; }
        public string TimeUpdated { get; set; }

        public string UserId { get; set; }
        public int PostId { get; set; }

        public ApplicationUser User { get; set; }

        public  Post Post { get; set; }
    }
}
