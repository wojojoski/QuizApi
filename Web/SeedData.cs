using ApplicationCore.Interfaces.Repository;
using BackendLab01;

namespace Infrastructure.Memory;
public static class SeedData
{
    public static void Seed(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            var quizRepo = provider.GetService<IGenericRepository<Quiz, int>>();
            var quizItemRepo = provider.GetService<IGenericRepository<QuizItem, int>>();

            var item1 = quizItemRepo.Add(new QuizItem(1, "2+2?", new List<string>() { "1", "2", "3" }, "4"));
            var item2 = quizItemRepo.Add(new QuizItem(2, "3+3?", new List<string>() { "20", "12", "49" }, "6"));
            var item3 = quizItemRepo.Add(new QuizItem(3, "4+4", new List<string>() { "4", "7", "2" }, "8"));
            quizRepo.Add(new Quiz(1, new List<QuizItem>() { item1, item2, item3 }, "Matematyka"));

            item1 = quizItemRepo.Add(new QuizItem(4, "5+5", new List<string>() { "8", "9", "11" }, "10"));
            item2 = quizItemRepo.Add(new QuizItem(5, "6+6", new List<string>() { "11", "13", "14" }, "12"));
            item3 = quizItemRepo.Add(new QuizItem(6, "7+7", new List<string>() { "15", "16", "17" }, "14"));
            quizRepo.Add(new Quiz(2, new List<QuizItem>() { item1, item2, item3 }, "Matematyka2"));
        }
    }
}