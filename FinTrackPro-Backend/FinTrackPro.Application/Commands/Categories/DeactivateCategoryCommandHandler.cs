using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Domain.Exceptions;
using MediatR;

namespace FinTrackPro.Application.Commands.Categories;

public sealed class DeactivateCategoryCommandHandler : IRequestHandler<DeactivateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeactivateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(DeactivateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, command.UserId, cancellationToken);

        if (category is null)
            throw new NotFoundException("Category", command.CategoryId);

        category.Deactivate();

        await _categoryRepository.SaveChangesAsync(cancellationToken);
    }
}
