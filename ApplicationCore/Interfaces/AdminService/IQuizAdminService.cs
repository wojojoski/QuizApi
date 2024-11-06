namespace BackendLab01;

public interface IQuizAdminService
{
    public QuizItem AddQuizItemToQuiz(int quizId, QuizItem item);
    public void UpdateQuizItem(int id, string question, List<string> incorrectAnswers, string correctAnswer, int points);
    public Quiz AddQuiz(Quiz quiz);
    public IEnumerable<QuizItem> FindAllQuizItems();
    public IEnumerable<Quiz> FindAllQuizzes();
    public void DeleteQuiz(int id);
}