using BackendLab01;
using WebAPI.DTO;

namespace WebAPI.Mapper
{
    public class QuizMapper
    {
        public static QuizItemDto MapItemDto(QuizItem item)
        {
            return new QuizItemDto()
            {
                Id = item.Id,
                Question = item.Question,
                Options = new List<string>(item.IncorrectAnswers)
                {
                    item.CorrectAnswer
                }
            };
        }

        public static QuizDto MapQuizDto (Quiz quiz)
        {
            return new QuizDto()
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Items = quiz.Items.Select(i => MapItemDto(i)).ToList(),
            };
        }
    }
}
