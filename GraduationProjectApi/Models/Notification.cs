using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GraduationProjectApi.Models;

namespace IdentityManagerServerApi.Models
{
    public class Notification
    {
        public int NotificationId { get; set; } // Renamed to NotificationId
        public string UserId { get; set; } // User ID of the recipient
        public string SenderUserId { get; set; } // User ID of the sender
        public int PostId { get; set; } // ID of the post related to the notification
        public NotificationType Type { get; set; } // Type of notification (e.g., Like, Comment)
        public bool IsRead { get; set; } // Indicates whether the notification has been read

        public string DateCreated { get; set; }
        public string TimeCreated { get; set; }
    }

    public enum NotificationType
    {
       
        Comment = 1 ,
        Like = 2 ,
    }



}
