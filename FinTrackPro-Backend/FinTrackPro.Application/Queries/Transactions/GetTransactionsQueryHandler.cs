using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Application.DTOs;
using MediatR;

namespace FinTrackPro.Application.Queries.Transactions;

public sealed class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, PaginatedList<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<PaginatedList<TransactionDto>> Handle(GetTransactionsQuery query, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _transactionRepository.GetPaginatedAsync(
            query.UserId,
            query.Page,
            query.PageSize,
            query.DateFrom,
            query.DateTo,
            query.TransactionType,
            query.CategoryId,
            query.AccountId,
            cancellationToken);

        var dtos = items.Select(TransactionDto.FromEntity).ToList();

        return new PaginatedList<TransactionDto>(dtos, query.Page, query.PageSize, totalCount);
    }
}
