using FinTrackPro.Application.DTOs;
using MediatR;

namespace FinTrackPro.Application.Commands.Categories;

public record CreateCategoryCommand(
    Guid UserId,
    string Name,
    string? Description,
    string? Icon,
    string? Color) : IRequest<CategoryDto>;
