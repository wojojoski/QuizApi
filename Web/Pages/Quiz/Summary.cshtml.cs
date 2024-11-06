using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackendLab01.Pages;

public class Summary : PageModel
{
    private readonly IQuizUserService _userService;
    public Summary(IQuizUserService userService)
    {
        _userService = userService; 
    }
    [BindProperty]
    public int QuizId {  get; set; }
    [BindProperty]
    public int ItemId {  get; set; }
    public void OnGet(int quizId, int itemId)
    {
        QuizId = quizId;
        ItemId = itemId;
        var result = _userService.CountCorrectAnswersForQuizFilledByUser(quizId, 1);
        TempData["correctAnswers"] = result.ToString();

    }
}