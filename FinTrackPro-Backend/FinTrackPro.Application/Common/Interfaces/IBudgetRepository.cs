using FinTrackPro.Domain.Entities;

namespace FinTrackPro.Application.Common.Interfaces;

public interface IBudgetRepository
{
    Task AddAsync(Budget budget, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Budget>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<Budget?> GetByIdAsync(Guid budgetId, Guid userId, CancellationToken cancellationToken);
}
