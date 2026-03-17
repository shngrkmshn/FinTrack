using MediatR;

namespace FinTrackPro.Application.Commands.Transactions;

public record DeleteTransactionCommand(
    Guid UserId,
    Guid TransactionId) : IRequest;
