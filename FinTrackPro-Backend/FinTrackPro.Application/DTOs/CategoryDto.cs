using FinTrackPro.Domain.Entities;

namespace FinTrackPro.Application.DTOs;

public sealed record CategoryDto(
    Guid Id,
    string Name,
    string? Description,
    string? Icon,
    string? Color,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static CategoryDto FromEntity(Category category)
    {
        return new CategoryDto(
            category.Id,
            category.Name,
            category.Description,
            category.Icon,
            category.Color,
            category.IsActive,
            category.CreatedAt,
            category.UpdatedAt);
    }
}
