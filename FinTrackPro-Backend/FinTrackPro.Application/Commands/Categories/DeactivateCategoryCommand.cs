using MediatR;

namespace FinTrackPro.Application.Commands.Categories;

public record DeactivateCategoryCommand(
    Guid UserId,
    Guid CategoryId) : IRequest;
