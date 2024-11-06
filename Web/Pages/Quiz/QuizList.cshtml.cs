using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackendLab01.Pages.Quiz
{
    public class QuizListModel : PageModel
    {
        private readonly IQuizAdminService _quizAdminService;
        public QuizListModel(IQuizAdminService quizAdminService)
        {
            _quizAdminService = quizAdminService;
        }

        public List<BackendLab01.Quiz> Quizzes { get; set; }
        public void OnGet()
        {
            Quizzes = _quizAdminService.FindAllQuizzes().ToList();
        }
    }
}
