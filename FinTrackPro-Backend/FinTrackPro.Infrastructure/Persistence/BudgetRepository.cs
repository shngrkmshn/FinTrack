using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrackPro.Infrastructure.Persistence;

public sealed class BudgetRepository : IBudgetRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BudgetRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Budget budget, CancellationToken cancellationToken) =>
        await _dbContext.Budgets.AddAsync(budget, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        _dbContext.SaveChangesAsync(cancellationToken);

    public async Task<IReadOnlyList<Budget>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken) =>
        await _dbContext.Budgets
            .Where(budget => budget.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task<Budget?> GetByIdAsync(Guid budgetId, Guid userId, CancellationToken cancellationToken) =>
        await _dbContext.Budgets
            .FirstOrDefaultAsync(budget => budget.Id == budgetId && budget.UserId == userId, cancellationToken);
}
