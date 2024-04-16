using BusinessLayer.Dto;
using BusinessLayer.Exceptions;
using BusinessLayer.Extensions;
using BusinessLayer.Mapping;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Implementations;

namespace BusinessLayer.Services.Implementations.AccountService;

public class AccountService : IAccountService
{
    private const int MinAccessLevelNumber = 0;
    private readonly DatabaseContext _context;

    public AccountService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<AccountDto> CreateAccount(int minAccessLevel, CancellationToken cancellationToken)
    {
        if (minAccessLevel < MinAccessLevelNumber)
            throw new BusinessLayerException("minAccessLevel can't be <= 0 while creating Account");

        var account = new Account(Guid.NewGuid(), minAccessLevel);

        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync(cancellationToken);

        return account.AsDto();
    }
}