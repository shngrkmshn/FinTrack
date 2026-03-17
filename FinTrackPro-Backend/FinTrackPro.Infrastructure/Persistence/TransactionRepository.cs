using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Domain.Entities;
using FinTrackPro.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinTrackPro.Infrastructure.Persistence;

public sealed class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TransactionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken) =>
        await _dbContext.Transactions.AddAsync(transaction, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        _dbContext.SaveChangesAsync(cancellationToken);

    public async Task<Transaction?> GetByIdAsync(Guid transactionId, Guid userId, CancellationToken cancellationToken) =>
        await _dbContext.Transactions
            .FirstOrDefaultAsync(
                transaction => transaction.Id == transactionId
                    && transaction.UserId == userId
                    && !transaction.IsDeleted,
                cancellationToken);

    public async Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPaginatedAsync(
        Guid userId,
        int page,
        int pageSize,
        DateTime? dateFrom,
        DateTime? dateTo,
        TransactionType? transactionType,
        Guid? categoryId,
        Guid? accountId,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Transactions
            .Where(transaction => transaction.UserId == userId && !transaction.IsDeleted);

        if (dateFrom.HasValue)
            query = query.Where(transaction => transaction.Date >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(transaction => transaction.Date <= dateTo.Value);

        if (transactionType.HasValue)
            query = query.Where(transaction => transaction.TransactionType == transactionType.Value);

        if (categoryId.HasValue)
            query = query.Where(transaction => transaction.CategoryId == categoryId.Value);

        if (accountId.HasValue)
            query = query.Where(transaction => transaction.AccountId == accountId.Value || transaction.ToAccountId == accountId.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(transaction => transaction.Date)
            .ThenByDescending(transaction => transaction.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
