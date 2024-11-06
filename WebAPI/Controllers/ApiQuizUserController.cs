using ApplicationCore.Exceptions;
using AutoMapper;
using BackendLab01;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics;
using WebAPI.DTO;
using WebAPI.Mapper;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/user/quizzes")]
    public class ApiQuizUserController : ControllerBase
    {
        private readonly IQuizUserService _service;
        private readonly IMapper _mapper;
        public ApiQuizUserController(IQuizUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        [Route("{id}")]
        [HttpGet]
        public ActionResult<QuizDto> FindById(int id)
        {
            Quiz? result = _service.FindQuizById(id);
            return result == null ? NotFound() :  _mapper.Map<QuizDto>(result);
                                                  //QuizMapper.MapQuizDto(result);
        }

        [HttpGet]
        public IEnumerable<QuizDto> FindAll()
        {
            var quizzes = _service.FindAllQuizzes();
            var quizDtos = new List<QuizDto>();
            foreach (var quiz in quizzes)
            {
                var quizDto = _mapper.Map<QuizDto>(quiz);
                                //QuizMapper.MapQuizDto(quiz);
                quizDtos.Add(quizDto);
            }
            return quizDtos;
        }

        [HttpPost]
        [Authorize(Policy = "Bearer")]
        [Route("{quizId}/items/{itemId}/answers")]
        public ActionResult SaveUserAnswer([FromBody] QuizItemUserAnswerDto dto, int quizId, int itemId)
        {
            try
            {
                var item = _service.SaveUserAnswerForQuiz(quizId, itemId ,dto.UserId, dto.Answer);

                string locationUri = Url.Action(nameof(GetQuizFeedback), new { quizId = quizId, userId = dto.UserId });
                return Created(locationUri, new { message = "Answer saved successfully!" });
            }
            catch (QuizAnswerItemAlreadyExistsException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (QuizNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", detail = ex.Message });
            }
        }


        [Route("{quizId}/answers/{userId}")]
        [HttpGet]
        public ActionResult<object> GetQuizFeedback(int quizId, int userId)
        {
            var feedback = _service.GetUserAnswersForQuiz(quizId, userId);
            return new
            {
                quizId = quizId,
                userId = userId,
                totalQuestion = _service.FindQuizById(quizId)?.Items.Count ?? 0,
                answers = feedback.Select(a =>
                new
                {
                    question = a.QuizItem.Question,
                    answer = a.Answer,
                    isCorrect = a.IsCorrect()
                }
                ).AsEnumerable()
            };
        }

    }
}
