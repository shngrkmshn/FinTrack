using FinTrackPro.Domain.Entities;
using FinTrackPro.Domain.Enums;

namespace FinTrackPro.Application.Common.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<Transaction?> GetByIdAsync(Guid transactionId, Guid userId, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPaginatedAsync(
        Guid userId,
        int page,
        int pageSize,
        DateTime? dateFrom,
        DateTime? dateTo,
        TransactionType? transactionType,
        Guid? categoryId,
        Guid? accountId,
        CancellationToken cancellationToken);
}
