using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO
{
    public class NewQuizDto
    {
        [Required]
        [Length(minimumLength: 3, maximumLength: 100)]
        public string Title { get; set; }
    }
}
