using BackendLab01;
using Infrastructure.MongoDB.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v2/quizzes")]
    public class ApiQuizMongoDBController : ControllerBase
    {
        private readonly QuizUserServiceMongoDB _quizService;
        public ApiQuizMongoDBController(QuizUserServiceMongoDB quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Quiz>> GetAllQuizzes()
        {
            var quizzes = _quizService.FindAllQuizzes();
            return Ok(quizzes);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Quiz> GetQuizById(int id)
        {
            var quiz = _quizService.FindQuizById(id);
            if (quiz == null)
            {
                return NotFound();
            }
            return Ok(quiz);
        }

        [HttpPost("answer")]
        public ActionResult<QuizItemUserAnswer> SubmitUserAnswer(int quizId, int quizItemId, int userId, string answer)
        {
            return Ok(_quizService.SaveUserAnswerForQuiz(quizId, quizItemId, userId, answer));
        }
        [HttpGet("{quizId}/user/{userId}/answers")]
        public ActionResult<List<QuizItemUserAnswer>> GetUserAnswers(int quizId, int userId)
        {
            return Ok(_quizService.GetUserAnswersForQuiz(quizId, userId));
        }
    }

}
