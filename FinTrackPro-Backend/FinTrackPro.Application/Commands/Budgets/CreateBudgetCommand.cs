using FinTrackPro.Application.DTOs;
using FinTrackPro.Domain.Enums;
using MediatR;

namespace FinTrackPro.Application.Commands.Budgets;

public record CreateBudgetCommand(
    Guid UserId,
    string Name,
    Guid CategoryId,
    decimal Amount,
    Currency Currency,
    DateTime PeriodStartDate,
    DateTime PeriodEndDate) : IRequest<BudgetDto>;
