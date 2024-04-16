namespace BusinessLayer.Dto;

public record CreateMobilePhoneModelDto(Guid AccountId, int PhoneNumber) : AbstractCreatorMessengerModelDto;