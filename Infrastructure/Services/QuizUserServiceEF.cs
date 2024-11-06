using ApplicationCore.Exceptions;
using BackendLab01;
using Infrastructure.EF.Entities;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class QuizUserServiceEF : IQuizUserService
    {
        private readonly QuizDbContext _dbContext;
        public QuizUserServiceEF(QuizDbContext quizDbContext)
        {
            _dbContext = quizDbContext;
        }

        public Quiz CreateAndGetQuizRandom(int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Quiz> FindAllQuizzes()
        {
            return _dbContext
                .Quizzes
                .AsNoTracking()
                .Include(q => q.Items)
                .ThenInclude(i => i.IncorrectAnswers)
                .Select(Mapper.FromEntityToQuiz)
                .ToList();
        }

        public Quiz? FindQuizById(int id)
        {
            var entity = _dbContext
                .Quizzes
                .AsNoTracking()
                .Include(q => q.Items)
                .ThenInclude(i => i.IncorrectAnswers)
                .FirstOrDefault(e => e.Id == id);
            return entity is null ? null : Mapper.FromEntityToQuiz(entity);
        }

        public List<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId)
        {
            var quizzEntity = _dbContext.Quizzes.AsNoTracking().FirstOrDefault(e => e.Id == quizId);
            if (quizzEntity == null)
                throw new QuizNotFoundException($"Quiz with id {quizId} not found");
            return _dbContext.UserAnswers.Include(a=>a.QuizItem).ThenInclude(i=>i.IncorrectAnswers)
                .Where(a=>a.UserId==userId&&a.QuizId==quizId).Select(Mapper.FromQuizItemUserAnswer).ToList();
        }

        public QuizItemUserAnswer SaveUserAnswerForQuiz(int quizId, int quizItemId, int userId, string answer)
        {
            QuizItemUserAnswerEntity entity = new QuizItemUserAnswerEntity()
            {
                UserId = userId,
                QuizItemId = quizItemId,
                QuizId = quizId,
                UserAnswer = answer
            };
            try
            {
                var saved = _dbContext.UserAnswers.Add(entity).Entity;
                var qzId = _dbContext.QuizItems.FirstOrDefault(x => x.Id == saved.QuizItemId);
                _dbContext.SaveChanges();
                return new QuizItemUserAnswer()
                {
                    UserId = saved.UserId,
                    QuizItem = Mapper.FromEntityToQuizItem(qzId),
                    QuizId = saved.QuizId,
                    Answer = saved.UserAnswer
                };
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException.Message.StartsWith("The INSERT"))
                {
                    throw new QuizNotFoundException("Quiz, quiz item or user not found. Can't save!");
                }
                if (e.InnerException.Message.StartsWith("Violation of"))
                {
                    throw new QuizAnswerItemAlreadyExistsException(quizId, quizItemId, userId);
                }
                throw new Exception(e.Message);
            }
        }
    }
}
