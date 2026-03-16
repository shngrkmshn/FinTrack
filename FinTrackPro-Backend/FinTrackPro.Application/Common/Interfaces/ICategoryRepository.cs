using FinTrackPro.Domain.Entities;

namespace FinTrackPro.Application.Common.Interfaces;

public interface ICategoryRepository
{
    Task AddAsync(Category category, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(Guid categoryId, Guid userId, CancellationToken cancellationToken);
}
