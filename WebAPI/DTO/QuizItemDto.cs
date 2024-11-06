namespace WebAPI.DTO
{
    public class QuizItemDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        
        public List<string> Options { get; set; }
    }
}
