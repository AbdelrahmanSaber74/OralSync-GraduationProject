using IdentityManagerServerApi.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityManagerServerApi.Models
{
    public class Rating
    {
        [Key]   
        public int RatingId { get; set; }

        // ID of the user who is being rated (receiver)
        public string RatedUserId { get; set; }

        // ID of the user who is providing the rating (sender)
        public string SenderUserId { get; set; }

        [Range(1, 5)]
        public int Value { get; set; } // Assuming the rating scale is from 1 to 5

        public DateTime CreatedAt { get; set; }


        // Navigation property for the rated user (receiver)
        [ForeignKey("RatedUserId")]
        public virtual ApplicationUser RatedUser { get; set; }

        // Navigation property for the sender user
        [ForeignKey("SenderUserId")]
        public virtual ApplicationUser SenderUser { get; set; }


    }
}
