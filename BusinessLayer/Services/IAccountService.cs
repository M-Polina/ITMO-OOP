using BusinessLayer.Dto;

namespace BusinessLayer.Services;

public interface IAccountService
{
    Task<AccountDto> CreateAccount(int minAccessLevel, CancellationToken cancellationToken);
}