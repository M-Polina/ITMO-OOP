using BusinessLayer.Dto;

namespace BusinessLayer.Mapping;

public static class CreateMobilePhoneModelMapping
{
    public static CreateMobilePhoneModelDto ToDto(Guid accountId, int number)
        => new CreateMobilePhoneModelDto(accountId, number);
}