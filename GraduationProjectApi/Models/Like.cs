using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GraduationProjectApi.Models;

namespace IdentityManagerServerApi.Models;
public class Like
{




    [Key]
    public int LikeId { get; set; }


    public string DateCreated { get; set; }
    public string TimeCreated { get; set; }

    public string DateUpdated { get; set; }
    public string TimeUpdated { get; set; }


    public string UserId { get; set; }




    // Foreign key property
    public int PostId { get; set; }

    // Navigation property for the post this comment belongs to
    [ForeignKey("PostId")]
    public virtual Post Post { get; set; }



}
