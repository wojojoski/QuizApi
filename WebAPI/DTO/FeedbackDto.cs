using BackendLab01;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace WebAPI.DTO
{
    public class FeedbackDto
    {
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public int TotalQuestion {  get; set; }
        public List<QuizItemUserAnswerDto> Answers{ get; set; }
    }
}
