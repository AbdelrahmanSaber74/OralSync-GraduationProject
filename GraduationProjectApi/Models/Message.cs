﻿namespace GraduationProjectApi.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string DateCreated { get; set; }
        public string TimeCreated { get; set; }
    }


}
