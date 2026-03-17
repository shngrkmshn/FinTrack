using FinTrackPro.Domain.Entities;
using FinTrackPro.Domain.Enums;

namespace FinTrackPro.Application.DTOs;

public sealed record BudgetDto(
    Guid Id,
    string Name,
    Guid CategoryId,
    Guid UserId,
    decimal Amount,
    Currency Currency,
    DateTime PeriodStartDate,
    DateTime PeriodEndDate,
    decimal SpentAmount,
    decimal SpentPercentage,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static BudgetDto FromEntity(Budget budget)
    {
        var spentPercentage = budget.Amount.Amount > 0
            ? Math.Round(budget.SpentAmount.Amount / budget.Amount.Amount * 100, 2)
            : 0m;

        return new BudgetDto(
            budget.Id,
            budget.Name,
            budget.CategoryId,
            budget.UserId,
            budget.Amount.Amount,
            budget.Amount.Currency,
            budget.Period.StartDate,
            budget.Period.EndDate,
            budget.SpentAmount.Amount,
            spentPercentage,
            budget.CreatedAt,
            budget.UpdatedAt);
    }
}
