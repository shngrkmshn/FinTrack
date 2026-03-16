using FinTrackPro.Domain.Entities;
using FinTrackPro.Domain.Enums;

namespace FinTrackPro.Application.DTOs;

public sealed record AccountDto(
    Guid Id,
    string Name,
    AccountType AccountType,
    decimal BalanceAmount,
    Currency BalanceCurrency,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive)
{
    public static AccountDto FromEntity(Account account)
    {
        return new AccountDto(
            account.Id,
            account.Name,
            account.AccountType,
            account.Balance.Amount,
            account.Balance.Currency,
            account.CreatedAt,
            account.UpdatedAt,
            account.IsActive);
    }
}
