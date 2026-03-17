using FinTrackPro.Application.DTOs;
using MediatR;

namespace FinTrackPro.Application.Queries.Budgets;

public record GetBudgetsQuery(Guid UserId) : IRequest<IReadOnlyList<BudgetDto>>;
