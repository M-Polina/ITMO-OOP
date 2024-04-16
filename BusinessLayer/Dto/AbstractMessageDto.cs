namespace BusinessLayer.Dto;

public record AbstractMessageDto(Guid MessageId, string Status, string Theme, string Message);