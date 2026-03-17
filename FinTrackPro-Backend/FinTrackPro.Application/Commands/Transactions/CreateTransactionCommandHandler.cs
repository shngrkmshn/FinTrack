using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Application.DTOs;
using FinTrackPro.Domain.Entities;
using FinTrackPro.Domain.Enums;
using FinTrackPro.Domain.Exceptions;
using FinTrackPro.Domain.ValueObjects;
using MediatR;

namespace FinTrackPro.Application.Commands.Transactions;

public sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateTransactionCommandHandler(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<TransactionDto> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        var categoryId = command.CategoryId;

        if (!categoryId.HasValue)
        {
            var uncategorized = await _categoryRepository.GetUncategorizedAsync(command.UserId, cancellationToken);

            if (uncategorized is null)
                throw new NotFoundException("Category", "Uncategorized");

            categoryId = uncategorized.Id;
        }

        var account = await _accountRepository.GetByIdAsync(command.AccountId, command.UserId, cancellationToken);

        if (account is null)
            throw new NotFoundException("Account", command.AccountId);

        var money = new Money(command.Amount, command.Currency);

        Transaction transaction;

        switch (command.TransactionType)
        {
            case TransactionType.Income:
                transaction = Transaction.CreateIncome(
                    money, command.Date, command.Description,
                    command.AccountId, command.UserId, categoryId.Value);
                account.Deposit(money);
                break;

            case TransactionType.Expense:
                transaction = Transaction.CreateExpense(
                    money, command.Date, command.Description,
                    command.AccountId, command.UserId, categoryId.Value);
                account.Withdraw(money);
                break;

            case TransactionType.Transfer:
                if (!command.ToAccountId.HasValue)
                    throw new ArgumentException("Transfer transactions must have a destination account.");

                var toAccount = await _accountRepository.GetByIdAsync(command.ToAccountId.Value, command.UserId, cancellationToken);

                if (toAccount is null)
                    throw new NotFoundException("Account", command.ToAccountId.Value);

                transaction = Transaction.CreateTransfer(
                    money, command.Date, command.Description,
                    command.AccountId, command.ToAccountId.Value,
                    command.UserId, categoryId.Value);
                account.Withdraw(money);
                toAccount.Deposit(money);
                break;

            default:
                throw new ArgumentException($"Invalid transaction type: {command.TransactionType}");
        }

        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _transactionRepository.SaveChangesAsync(cancellationToken);

        return TransactionDto.FromEntity(transaction);
    }
}
