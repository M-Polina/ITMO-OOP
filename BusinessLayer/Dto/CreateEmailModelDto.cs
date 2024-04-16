namespace BusinessLayer.Dto;


public record CreateEmailModelDto(Guid AccountId, string Name) : AbstractCreatorMessengerModelDto;