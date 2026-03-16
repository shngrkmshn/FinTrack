using System.Security.Claims;
using FinTrackPro.Application.Commands.Accounts;
using FinTrackPro.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrackPro.API.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize]
public sealed class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(
        [FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var command = new CreateAccountCommand(userId, request.Name, request.AccountType, request.Currency);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

}

public sealed record CreateAccountRequest(string Name, AccountType AccountType, Currency Currency);
