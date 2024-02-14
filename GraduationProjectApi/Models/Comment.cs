using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GraduationProjectApi.Models;

namespace IdentityManagerServerApi.Models;
public class Comment
{
    [Key]
    public int CommentId { get; set; }

    [Required]
    public string Content { get; set; }
    public string DateCreated { get; set; }
    public string TimeCreated { get; set; }

    public string DateUpdated { get; set; }
    public string TimeUpdated { get; set; }


    [Required]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }


    // Foreign key property
    [Required]
    public int PostId { get; set; }

    // Navigation property for the post this comment belongs to
    [ForeignKey("PostId")]
    public virtual Post Post { get; set; }





}
