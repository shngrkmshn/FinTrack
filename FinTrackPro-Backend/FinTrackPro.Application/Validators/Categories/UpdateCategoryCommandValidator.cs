using FinTrackPro.Application.Commands.Categories;
using FluentValidation;

namespace FinTrackPro.Application.Validators.Categories;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.CategoryId)
            .NotEmpty();

        RuleFor(command => command.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(command => command.Description)
            .MaximumLength(500)
            .When(command => command.Description is not null);

        RuleFor(command => command.Color)
            .Matches(@"^#([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$")
            .When(command => command.Color is not null);
    }
}
