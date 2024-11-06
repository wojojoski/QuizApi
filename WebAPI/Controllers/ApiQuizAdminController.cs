using AutoMapper;
using BackendLab01;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTO;

namespace WebAPI.Controllers
{
    [Controller]
    [Route("/api/v1/admin/quizzes")]
    public class ApiQuizAdminController : ControllerBase
    {
        private readonly IQuizAdminService _quizAdminService;
        private readonly IMapper _mapper;
        private readonly IValidator<QuizItem> _quizItemValidator;
        private readonly LinkGenerator _linkGenerator;
        public ApiQuizAdminController(IQuizAdminService quizAdminService, IMapper mapper, IValidator<QuizItem> quizItemValidator, LinkGenerator linkGenerator)
        {
            _quizAdminService = quizAdminService;
            _mapper = mapper;
            _quizItemValidator = quizItemValidator;
            _linkGenerator = linkGenerator;
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult<Quiz> GetById(int id)
        {
            var quiz = _quizAdminService.FindAllQuizzes().FirstOrDefault(q => q.Id == id);
            return quiz is null ? NotFound() : Ok(quiz);
        }
        [HttpGet]
        public ActionResult GetAllQuizzes()
        {
            var quizzes = _quizAdminService.FindAllQuizzes();
            return quizzes == null ? NotFound() : Ok(quizzes);
        }
        [HttpPost]
        public ActionResult CreateQuiz([FromBody]NewQuizDto dto)
        {
            var quiz = _mapper.Map<Quiz>(dto);
            var createdQuiz = _quizAdminService.AddQuiz(quiz);

            return Created(_linkGenerator.GetUriByAction(HttpContext, nameof(GetById), null, new { id = quiz.Id }), createdQuiz);
        }
        [HttpPatch]
        [Route("{quizId}")]
        [Consumes("application/json-patch+json")]
        public ActionResult AddQuizItem(int quizId, [FromBody] JsonPatchDocument<Quiz>? patchDoc)
        {
            var quiz = _quizAdminService.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
            if (quiz is null || patchDoc is null)
            {
                return NotFound(new
                {
                    error = $"Quiz with id {quizId} not found"
                });
            }
            int previousCount = quiz.Items.Count();
            patchDoc.ApplyTo(quiz, ModelState);
            int currentCount = quiz.Items.Count();
            TryValidateModel(quiz);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (previousCount < quiz.Items.Count)
            {
                QuizItem item = quiz.Items[^1];
                quiz.Items.RemoveAt(quiz.Items.Count - 1);
                var validationResult = _quizItemValidator.Validate(item);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
                }
                _quizAdminService.AddQuizItemToQuiz(quizId, item);
            }
            return Ok(_quizAdminService.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId));
        }


        [HttpDelete]
        [Route("{quizId}")]
        public ActionResult DeleteQuizById(int quizId)
        {
            var quiz = _quizAdminService.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
            if (quiz == null)
            {
                return NotFound();
            }
            _quizAdminService.DeleteQuiz(quizId);
            return Ok(quiz);
        }
    }
}
