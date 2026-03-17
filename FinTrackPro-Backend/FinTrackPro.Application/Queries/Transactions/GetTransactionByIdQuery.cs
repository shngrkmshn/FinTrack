using FinTrackPro.Application.DTOs;
using MediatR;

namespace FinTrackPro.Application.Queries.Transactions;

public record GetTransactionByIdQuery(
    Guid UserId,
    Guid TransactionId) : IRequest<TransactionDto>;
