using BackendLab01;
using FluentValidation;
using WebAPI.DTO;

namespace WebAPI.Validators
{
    public class QuizItemValidator : AbstractValidator<QuizItem>
    {
        public QuizItemValidator()
        {
            RuleFor(q => q.Question)
                .MaximumLength(200).WithMessage("The question cannot be longer than 200 characters.")
                .MinimumLength(2).WithMessage("The question cannot be shorter than 2 characters!");

            RuleForEach(q => q.IncorrectAnswers)
                .NotEmpty().WithMessage("Each incorrect answer must not be empty.")
                .MaximumLength(200).WithMessage("Each incorrect answer cannot be longer than 200 characters.");

            RuleFor(q => q.CorrectAnswer)
                .NotEmpty().WithMessage("The correct answer must not be empty.")
                .MaximumLength(200).WithMessage("The correct answer cannot be longer than 200 characters.");

            RuleFor(q => new { q.CorrectAnswer, q.IncorrectAnswers })
                .Must(t => !t.IncorrectAnswers.Contains(t.CorrectAnswer))
                .WithMessage("The correct answer should not appear in the list of incorrect answers!");

            RuleFor(q => q.IncorrectAnswers)
                .Must(i => i.Count > 0).WithMessage("There must be at least one incorrect answer.");
        }
    }
}
