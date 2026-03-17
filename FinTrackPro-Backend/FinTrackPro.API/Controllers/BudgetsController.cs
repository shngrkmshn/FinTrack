using System.Security.Claims;
using FinTrackPro.Application.Commands.Budgets;
using FinTrackPro.Application.Queries.Budgets;
using FinTrackPro.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackPro.API.Controllers;

[ApiController]
[Route("api/budgets")]
[Authorize]
public sealed class BudgetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBudgets(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var query = new GetBudgetsQuery(userId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBudget(
        [FromBody] CreateBudgetRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var command = new CreateBudgetCommand(
            userId,
            request.Name,
            request.CategoryId,
            request.Amount,
            request.Currency,
            request.PeriodStartDate,
            request.PeriodEndDate);

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}

public sealed record CreateBudgetRequest(
    string Name,
    Guid CategoryId,
    decimal Amount,
    Currency Currency,
    DateTime PeriodStartDate,
    DateTime PeriodEndDate);
