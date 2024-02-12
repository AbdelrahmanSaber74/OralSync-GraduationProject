using System.ComponentModel.DataAnnotations;


namespace SharedClassLibrary.DTOs
{
    public class PostDto
    {

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
 


    }
}
