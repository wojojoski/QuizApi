using ApplicationCore.Interfaces.Repository;

namespace BackendLab01;

public class QuizAdminService:IQuizAdminService
{
    private IGenericRepository<Quiz, int> quizRepository;
    private IGenericRepository<QuizItem, int> itemRepository;

    public QuizAdminService(IGenericRepository<Quiz, int> quizRepository, IGenericRepository<QuizItem, int> itemRepository)
    {
        this.quizRepository = quizRepository;
        this.itemRepository = itemRepository;
    }

    public QuizItem AddQuizItemToQuiz(int quizId, QuizItem item)
    {
        var quiz = quizRepository.FindById(quizId);
        if (quiz is null)
        {
            throw new Exception();
        }
        var newItem = itemRepository.Add(item);
        quiz.Items.Add(newItem);
        quizRepository.Update(quizId, quiz);
        return newItem;
    }

    public void UpdateQuizItem(int id, string question, List<string> incorrectAnswers, string correctAnswer, int points)
    {
        var quizItem = new QuizItem(id: id, question: question, incorrectAnswers: incorrectAnswers, correctAnswer: correctAnswer);
        itemRepository.Update(id, quizItem);
    }
    public Quiz AddQuiz(Quiz quiz)
    {
        return quizRepository.Add(quiz);
    }

    public IEnumerable<QuizItem> FindAllQuizItems()
    {
        return itemRepository.FindAll();
    }

    public IEnumerable<Quiz> FindAllQuizzes()
    { return quizRepository.FindAll();
    }
    public void DeleteQuiz(int id)
    {
        quizRepository.RemoveById(id);
    }
}