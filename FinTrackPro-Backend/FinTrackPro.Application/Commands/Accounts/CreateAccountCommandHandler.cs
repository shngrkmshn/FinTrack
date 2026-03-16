using FinTrackPro.Application.Common.Interfaces;
using FinTrackPro.Application.DTOs;
using FinTrackPro.Domain.Entities;
using MediatR;

namespace FinTrackPro.Application.Commands.Accounts;

public sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountDto>
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var account = Account.Create(command.Name, command.AccountType, command.Currency, command.UserId);

        await _accountRepository.AddAsync(account, cancellationToken);
        await _accountRepository.SaveChangesAsync(cancellationToken);

        return AccountDto.FromEntity(account);
    }
}
