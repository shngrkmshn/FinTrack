using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrackPro.Infrastructure.Persistence;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken) =>
        await _dbContext.Categories.AddAsync(category, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        _dbContext.SaveChangesAsync(cancellationToken);

    public async Task<IReadOnlyList<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken) =>
        await _dbContext.Categories
            .Where(category => category.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task<Category?> GetByIdAsync(Guid categoryId, Guid userId, CancellationToken cancellationToken) =>
        await _dbContext.Categories
            .FirstOrDefaultAsync(category => category.Id == categoryId && category.UserId == userId, cancellationToken);
}
