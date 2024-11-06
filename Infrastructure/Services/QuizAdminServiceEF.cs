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
    public class QuizAdminServiceEF : IQuizAdminService
    {
        private readonly QuizDbContext _dbContext;

        public QuizAdminServiceEF(QuizDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Quiz AddQuiz(Quiz quiz)
        {
            var quizEntity = Mapper.FromQuizToQuizEntity(quiz);
            _dbContext.Quizzes.Add(quizEntity);
            _dbContext.SaveChanges();
            return Mapper.FromEntityToQuiz(quizEntity);
        }

        public QuizItem AddQuizItemToQuiz(int quizId, QuizItem item)
        {
            var quiz = FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
            var quizItemEntity = Mapper.FromQuizItemToQuizItemEntity(item);
            _dbContext.QuizItems.Add(quizItemEntity);
            quiz.Items.Add(item);
            var test = new List<QuizItemEntity>()
            {
                quizItemEntity
            };
            ISet<QuizItemEntity> qie = new HashSet<QuizItemEntity>(test);
            QuizEntity quizEntity = new QuizEntity
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Items = qie
            };

            //_dbContext.ChangeTracker.Clear();
            //_dbContext.Entry(quiz).State = EntityState.Modified;
            _dbContext.Update(quizEntity);
            _dbContext.SaveChanges();
            return Mapper.FromEntityToQuizItem(quizItemEntity);
        }

        public IEnumerable<QuizItem> FindAllQuizItems()
        {
            return _dbContext
                .QuizItems
                .Select(Mapper.FromEntityToQuizItem);
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

        public void UpdateQuizItem(int id, string question, List<string> incorrectAnswers, string correctAnswer, int points)
        {
            throw new NotImplementedException();
        }

        public void DeleteQuiz(int id)
        {
            var quizEntity = _dbContext.Quizzes.FirstOrDefault(q => q.Id == id);
            if (quizEntity != null)
            {
                _dbContext.Quizzes.Remove(quizEntity);
                _dbContext.SaveChanges();
            }
        }
    }
}
