using BackendLab01;
using Infrastructure.EF.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MongoDB.Entities
{
    public class AnswerMongoEntity : BaseMongoEntity
    {
        [BsonElement("quizId")]
        public int QuizId { get; set; }
        [BsonElement("quizItemId")]
        public int ItemId { get; set; }
        [BsonElement("userId")]
        public int UserId { get; set; }

        [BsonElement("userAnswer")]
        public String UserAnswer { get; set; }


        public AnswerMongoEntity()
        {

        }
        public AnswerMongoEntity(int quizId, int userId, int itemId, string userAnswer)
        {
            QuizId = quizId;
            UserId = userId;
            ItemId = itemId;
            UserAnswer = userAnswer;
        }
    }
}
