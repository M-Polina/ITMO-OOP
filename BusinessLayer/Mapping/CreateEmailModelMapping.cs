using BusinessLayer.Dto;
using DataAccessLayer.Models.Implementations;

namespace BusinessLayer.Mapping;

public static class CreateEmailModelMapping
{
    public static CreateEmailModelDto ToDto(Guid accountId, string name)
        => new CreateEmailModelDto(accountId, name);
}