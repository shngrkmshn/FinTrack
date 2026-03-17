using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Domain.Enums;
using FinTrackPro.Domain.Exceptions;
using MediatR;

namespace FinTrackPro.Application.Commands.Transactions;

public sealed class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;

    public DeleteTransactionCommandHandler(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task Handle(DeleteTransactionCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(command.TransactionId, command.UserId, cancellationToken);

        if (transaction is null)
            throw new NotFoundException("Transaction", command.TransactionId);

        var account = await _accountRepository.GetByIdAsync(transaction.AccountId, command.UserId, cancellationToken);

        if (account is null)
            throw new NotFoundException("Account", transaction.AccountId);

        switch (transaction.TransactionType)
        {
            case TransactionType.Income:
                account.Withdraw(transaction.Amount);
                break;

            case TransactionType.Expense:
                account.Deposit(transaction.Amount);
                break;

            case TransactionType.Transfer:
                account.Deposit(transaction.Amount);

                var toAccount = await _accountRepository.GetByIdAsync(transaction.ToAccountId!.Value, command.UserId, cancellationToken);

                if (toAccount is null)
                    throw new NotFoundException("Account", transaction.ToAccountId.Value);

                toAccount.Withdraw(transaction.Amount);
                break;
        }

        transaction.Delete();

        await _transactionRepository.SaveChangesAsync(cancellationToken);
    }
}
