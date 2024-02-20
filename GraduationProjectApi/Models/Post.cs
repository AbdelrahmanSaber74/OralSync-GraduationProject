using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraduationProjectApi.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        // Date and time properties for creation and update timestamps
        public string DateCreated { get; set; }
        public string TimeCreated { get; set; }
        public string DateUpdated { get; set; }
        public string TimeUpdated { get; set; }

        // Property to indicate whether the post is visible or hidden
        public bool IsVisible { get; set; }

        // Image property with question mark indicates it's nullable
        public List<string>  Image { get; set; }

        // Foreign key for the user who authored the post
        public string UserId { get; set; }

        // Navigation property for the user who authored the post
        public ApplicationUser User { get; set; }

        // Navigation property for comments related to this post
        public virtual ICollection<Comment> Comments { get; set; }

        // Navigation property for likes related to this post
        public virtual ICollection<Like> Likes { get; set; }


    }
}
