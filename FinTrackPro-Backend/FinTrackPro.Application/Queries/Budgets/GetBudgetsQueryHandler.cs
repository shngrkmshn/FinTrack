using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Application.DTOs;
using MediatR;

namespace FinTrackPro.Application.Queries.Budgets;

public sealed class GetBudgetsQueryHandler : IRequestHandler<GetBudgetsQuery, IReadOnlyList<BudgetDto>>
{
    private readonly IBudgetRepository _budgetRepository;

    public GetBudgetsQueryHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<IReadOnlyList<BudgetDto>> Handle(GetBudgetsQuery query, CancellationToken cancellationToken)
    {
        var budgets = await _budgetRepository.GetByUserIdAsync(query.UserId, cancellationToken);
        return budgets.Select(BudgetDto.FromEntity).ToList();
    }
}
