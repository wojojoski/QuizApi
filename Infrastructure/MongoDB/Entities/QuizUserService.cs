using BackendLab01;
using Infrastructure.EF.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MongoDB.Entities
{
    public class QuizUserServiceMongoDB : IQuizUserService
    {
        private readonly IMongoCollection<QuizMongoEntity> _quizzes;
        private readonly IMongoCollection<AnswerMongoEntity> _answers;
        private readonly MongoClient _client;

        public QuizUserServiceMongoDB(IOptions<MongoDBSettings> settings)
        {
            _client = new MongoClient(settings.Value.ConnectionUri);
            IMongoDatabase database = _client.GetDatabase(settings.Value.DatabaseName);
            _quizzes = database.GetCollection<QuizMongoEntity>(settings.Value.QuizCollection);
            _answers = database.GetCollection<AnswerMongoEntity>(settings.Value.AnswersCollection);
        }

        public IEnumerable<Quiz> FindAllQuizzes()
        {
            var quizMongoEntities = _quizzes.Find(Builders<QuizMongoEntity>.Filter.Empty).ToList();
            return _quizzes
                .Find(Builders<QuizMongoEntity>.Filter.Empty)
                .Project(
                    q =>

                    new Quiz(
                            q.QuizId,
                            q.Items.Select(i => new QuizItem(
                                    i.ItemId,
                                    i.Question,
                                    i.IncorrectAnswers,
                                    i.CorrectAnswer
                                )
                            ).ToList(),
                            q.Title
                        )
                ).ToEnumerable();
        }

        public Quiz? FindQuizById(int id)
        {
            return _quizzes
                .Find(Builders<QuizMongoEntity>.Filter.Eq(q => q.QuizId, id))
                .Project(q =>
                    new Quiz(
                        q.QuizId,
                        q.Items.Select(i => new QuizItem(
                                i.ItemId,
                                i.Question,
                                i.IncorrectAnswers,
                                i.CorrectAnswer
                            )
                        ).ToList(),
                        q.Title
                    )
                ).FirstOrDefault();
        }

        public QuizItemUserAnswer SaveUserAnswerForQuiz(int quizId, int quizItemId, int userId, string answer)
        {
            var userAnswer = new AnswerMongoEntity
            {
                QuizId = quizId,
                UserId = userId,
                ItemId = quizItemId,
                UserAnswer = answer
            };

            _answers.InsertOne(userAnswer);

            var quizIdFilter = Builders<QuizMongoEntity>.Filter.Eq(i => i.QuizId, quizId);

            var quizMongoDb = _quizzes.Find(quizIdFilter).FirstOrDefault();

            if (quizMongoDb != null)
            {
                var quizItemMongoDb = quizMongoDb.Items.FirstOrDefault(item => item.ItemId == quizItemId);

                if (quizItemMongoDb == null)
                {
                    throw new Exception($"Quiz item with ID {quizItemId} not found.");
                }


                var quizItem = new QuizItem(
                quizItemMongoDb.ItemId,
                quizItemMongoDb.Question,
                quizItemMongoDb.IncorrectAnswers,
                quizItemMongoDb.CorrectAnswer
                );

                return new QuizItemUserAnswer(
                quizItem,
                userAnswer.UserId,
                userAnswer.QuizId,
                userAnswer.UserAnswer
                );
            }
            else
            {
                throw new Exception("Error!");
            }  
        }

        public List<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId)
        {
            var userAnswers = _answers.Find(Builders<AnswerMongoEntity>.Filter.Eq(a => a.QuizId, quizId) &
                                             Builders<AnswerMongoEntity>.Filter.Eq(a => a.UserId, userId))
                                      .ToList();

            var quizMongoDb = _quizzes.Find(Builders<QuizMongoEntity>.Filter.Eq(q => q.QuizId, quizId)).FirstOrDefault();

            var quizItemUserAnswers = new List<QuizItemUserAnswer>();

            foreach (var userAnswer in userAnswers)
            {
                var quizItemMongoDb = quizMongoDb.Items.FirstOrDefault(item => item.ItemId == userAnswer.ItemId);

                if (quizItemMongoDb != null)
                {
                    var quizItem = new QuizItem(
                        quizItemMongoDb.ItemId,
                        quizItemMongoDb.Question,
                        quizItemMongoDb.IncorrectAnswers,
                        quizItemMongoDb.CorrectAnswer
                    );

                    var quizItemUserAnswer = new QuizItemUserAnswer(
                        quizItem,
                        userAnswer.UserId,
                        userAnswer.QuizId,
                        userAnswer.UserAnswer
                    );

                    quizItemUserAnswers.Add(quizItemUserAnswer);
                }
            }

            return quizItemUserAnswers;
        }

        public Quiz CreateAndGetQuizRandom(int count)
        {
            throw new NotImplementedException();
        }
    }
}
