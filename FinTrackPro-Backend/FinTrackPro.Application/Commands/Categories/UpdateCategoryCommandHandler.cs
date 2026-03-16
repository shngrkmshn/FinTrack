using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Application.DTOs;
using FinTrackPro.Domain.Exceptions;
using MediatR;

namespace FinTrackPro.Application.Commands.Categories;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, command.UserId, cancellationToken);

        if (category is null)
            throw new NotFoundException("Category", command.CategoryId);

        category.UpdateName(command.Name);
        category.UpdateDescription(command.Description);
        category.UpdateAppearance(command.Icon, command.Color);

        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return CategoryDto.FromEntity(category);
    }
}
