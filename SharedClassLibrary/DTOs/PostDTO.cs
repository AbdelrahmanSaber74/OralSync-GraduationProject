using System.ComponentModel.DataAnnotations;

namespace SharedClassLibrary.DTOs
{
    public class PostDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
    }
}
