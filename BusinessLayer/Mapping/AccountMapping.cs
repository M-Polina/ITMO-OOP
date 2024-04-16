using BusinessLayer.Dto;
using DataAccessLayer.Models.Implementations;

namespace BusinessLayer.Mapping;

public static class AccountMapping
{
    public static AccountDto AsDto(this Account account)
        => new AccountDto(account.Id, account.MinAccessLevel);
}