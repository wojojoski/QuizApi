using BackendLab01;
using Infrastructure.EF.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappers
{
    public static class Mapper
    {
        public static QuizItem FromEntityToQuizItem(QuizItemEntity entity)
        {
            return new QuizItem(
                entity.Id,
                entity.Question,
                entity.IncorrectAnswers.Select(e => e.Answer).ToList(),
                entity.CorrectAnswer);
        }

        public static Quiz FromEntityToQuiz(QuizEntity entity)
        {
            return new Quiz
            {
                Id = entity.Id,
                Title = entity.Title,
                Items = entity.Items.Select(x => FromEntityToQuizItem(x)).ToList()
            };
        }

        public static QuizItemUserAnswer FromQuizItemUserAnswer(QuizItemUserAnswerEntity entity)
        {
            var result = new QuizItemUserAnswer()
            {
                QuizId = entity.QuizId,
                UserId = entity.UserId,
                Answer = entity.UserAnswer,
                QuizItem = FromEntityToQuizItem(entity.QuizItem)
            };
            return result;
        }

        public static QuizEntity FromQuizToQuizEntity(Quiz quiz)
        {
            var result = new QuizEntity()
            {
                Title = quiz.Title,
            };
            return result;
        } 

        public static QuizItemEntity FromQuizItemToQuizItemEntity(QuizItem quizItem)
        {
            var result = new QuizItemEntity()
            {
                Question = quizItem.Question,
                CorrectAnswer = quizItem.CorrectAnswer,
                IncorrectAnswers = quizItem.IncorrectAnswers.Select(answer => new QuizItemAnswerEntity
                {
                    Answer = answer,
                }).ToHashSet()
            };
            return result;
        }
    }
}
