using FinTrackPro.Application.DTOs;
using FinTrackPro.Domain.Enums;
using MediatR;

namespace FinTrackPro.Application.Commands.Transactions;

public record CreateTransactionCommand(
    Guid UserId,
    TransactionType TransactionType,
    decimal Amount,
    Currency Currency,
    DateTime Date,
    string Description,
    Guid AccountId,
    Guid? CategoryId,
    Guid? ToAccountId) : IRequest<TransactionDto>;
