using ApplicationCore.Interfaces.Repository;

namespace BackendLab01;

public class QuizItemUserAnswer: IIdentity<string>
{
    public int QuizId { get; set; }
    public QuizItem  QuizItem{ get; set; }
    public int UserId { get; set; }
    public string Answer { get; set; }
    public QuizItemUserAnswer(QuizItem quizItem, int userId, int quizId,string answer)
    {
        QuizItem = quizItem;
        Answer = answer;
        UserId = userId;
        QuizId = quizId;
    }

    public bool IsCorrect()
    {
        return QuizItem.CorrectAnswer == Answer;
    }

    public string Id
    {
        get => $"{QuizId}{UserId}{QuizItem.Id}";
        set
        {
            
        }
    }
    public QuizItemUserAnswer()
    {

    }
}