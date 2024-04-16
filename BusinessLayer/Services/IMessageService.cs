using BusinessLayer.Dto;

namespace BusinessLayer.Services;

public interface IMessageService
{
    Task<EmailMessageDto> CreateEmailMessage(
        Guid messengerId,
        string message,
        string theme,
        CancellationToken cancellationToken);

    Task<PhoneMessageDto> CreateMobilePhoneMessage(
        Guid messengerId,
        string message,
        CancellationToken cancellationToken);
}