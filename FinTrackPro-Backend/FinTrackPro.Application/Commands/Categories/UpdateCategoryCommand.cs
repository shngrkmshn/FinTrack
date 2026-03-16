using FinTrackPro.Application.DTOs;
using MediatR;

namespace FinTrackPro.Application.Commands.Categories;

public record UpdateCategoryCommand(
    Guid UserId,
    Guid CategoryId,
    string Name,
    string? Description,
    string? Icon,
    string? Color) : IRequest<CategoryDto>;
