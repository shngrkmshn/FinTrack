using FinTrackPro.Domain.Entities;
using FinTrackPro.Domain.Enums;

namespace FinTrackPro.Application.DTOs;

public sealed record TransactionDto(
    Guid Id,
    TransactionType TransactionType,
    decimal Amount,
    Currency Currency,
    DateTime Date,
    string Description,
    Guid AccountId,
    Guid CategoryId,
    Guid? ToAccountId,
    Guid? RecurrenceScheduleId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsDeleted,
    DateTime? DeletedAt)
{
    public static TransactionDto FromEntity(Transaction transaction)
    {
        return new TransactionDto(
            transaction.Id,
            transaction.TransactionType,
            transaction.Amount.Amount,
            transaction.Amount.Currency,
            transaction.Date,
            transaction.Description,
            transaction.AccountId,
            transaction.CategoryId,
            transaction.ToAccountId,
            transaction.RecurrenceScheduleId,
            transaction.CreatedAt,
            transaction.UpdatedAt,
            transaction.IsDeleted,
            transaction.DeletedAt);
    }
}
