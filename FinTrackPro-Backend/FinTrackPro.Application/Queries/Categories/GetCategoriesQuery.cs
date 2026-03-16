using FinTrackPro.Application.DTOs;
using MediatR;

namespace FinTrackPro.Application.Queries.Categories;

public record GetCategoriesQuery(Guid UserId) : IRequest<IReadOnlyList<CategoryDto>>;
