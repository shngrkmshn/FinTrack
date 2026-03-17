using FinTrackPro.Application.Commands.Transactions;
using FinTrackPro.Domain.Enums;
using FluentValidation;

namespace FinTrackPro.Application.Validators.Transactions;

public sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.TransactionType)
            .IsInEnum();

        RuleFor(command => command.Amount)
            .GreaterThan(0);

        RuleFor(command => command.Currency)
            .IsInEnum();

        RuleFor(command => command.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(command => command.AccountId)
            .NotEmpty();

        RuleFor(command => command.ToAccountId)
            .NotEmpty()
            .When(command => command.TransactionType == TransactionType.Transfer)
            .WithMessage("Transfer transactions must have a destination account.");

        RuleFor(command => command.ToAccountId)
            .Null()
            .When(command => command.TransactionType != TransactionType.Transfer)
            .WithMessage("Only transfer transactions can have a destination account.");
    }
}
