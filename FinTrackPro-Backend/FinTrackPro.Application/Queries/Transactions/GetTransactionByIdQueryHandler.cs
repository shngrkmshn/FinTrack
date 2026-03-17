using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Application.DTOs;
using FinTrackPro.Domain.Exceptions;
using MediatR;

namespace FinTrackPro.Application.Queries.Transactions;

public sealed class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionDto> Handle(GetTransactionByIdQuery query, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(query.TransactionId, query.UserId, cancellationToken);

        if (transaction is null)
            throw new NotFoundException("Transaction", query.TransactionId);

        return TransactionDto.FromEntity(transaction);
    }
}
