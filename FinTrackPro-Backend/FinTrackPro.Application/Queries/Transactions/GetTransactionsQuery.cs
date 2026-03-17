using FinTrackPro.Application.DTOs;
using FinTrackPro.Domain.Enums;
using MediatR;

namespace FinTrackPro.Application.Queries.Transactions;

public record GetTransactionsQuery(
    Guid UserId,
    int Page,
    int PageSize,
    DateTime? DateFrom,
    DateTime? DateTo,
    TransactionType? TransactionType,
    Guid? CategoryId,
    Guid? AccountId) : IRequest<PaginatedList<TransactionDto>>;
