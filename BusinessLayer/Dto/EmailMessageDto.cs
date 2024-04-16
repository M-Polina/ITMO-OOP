namespace BusinessLayer.Dto;

public record EmailMessageDto(Guid MessageId, string Status, string Theme, string Message);