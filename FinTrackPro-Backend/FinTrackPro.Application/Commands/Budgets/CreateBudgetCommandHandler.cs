using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Application.DTOs;
using FinTrackPro.Domain.Entities;
using FinTrackPro.Domain.ValueObjects;
using MediatR;

namespace FinTrackPro.Application.Commands.Budgets;

public sealed class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, BudgetDto>
{
    private readonly IBudgetRepository _budgetRepository;

    public CreateBudgetCommandHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<BudgetDto> Handle(CreateBudgetCommand command, CancellationToken cancellationToken)
    {
        var amount = new Money(command.Amount, command.Currency);
        var period = new DateRange(command.PeriodStartDate, command.PeriodEndDate);

        var budget = Budget.Create(
            command.Name,
            command.CategoryId,
            command.UserId,
            amount,
            period);

        await _budgetRepository.AddAsync(budget, cancellationToken);
        await _budgetRepository.SaveChangesAsync(cancellationToken);

        return BudgetDto.FromEntity(budget);
    }
}
