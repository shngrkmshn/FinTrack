using FinTrackPro.Application.Commands.Budgets;
using FluentValidation;

namespace FinTrackPro.Application.Validators.Budgets;

public sealed class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.CategoryId)
            .NotEmpty();

        RuleFor(command => command.Amount)
            .GreaterThan(0);

        RuleFor(command => command.Currency)
            .IsInEnum();

        RuleFor(command => command.PeriodStartDate)
            .NotEmpty();

        RuleFor(command => command.PeriodEndDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(command => command.PeriodStartDate)
            .WithMessage("Period end date must be on or after start date.");
    }
}
