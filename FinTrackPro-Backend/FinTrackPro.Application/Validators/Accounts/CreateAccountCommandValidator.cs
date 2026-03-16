using FinTrackPro.Application.Commands.Accounts;
using FluentValidation;

namespace FinTrackPro.Application.Validators.Accounts;

public sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.AccountType)
            .IsInEnum();

        RuleFor(command => command.Currency)
            .IsInEnum();
    }
}
