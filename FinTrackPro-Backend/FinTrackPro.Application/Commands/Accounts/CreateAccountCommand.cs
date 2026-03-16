using FinTrackPro.Application.DTOs;
using FinTrackPro.Domain.Enums;
using MediatR;

namespace FinTrackPro.Application.Commands.Accounts;

public record CreateAccountCommand(
    Guid UserId,
    string Name,
    AccountType AccountType,
    Currency Currency) : IRequest<AccountDto>;
