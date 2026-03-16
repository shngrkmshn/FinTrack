using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Application.DTOs;
using MediatR;

namespace FinTrackPro.Application.Queries.Categories;

public sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyList<CategoryDto>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetByUserIdAsync(query.UserId, cancellationToken);
        return categories.Select(CategoryDto.FromEntity).ToList();
    }
}
