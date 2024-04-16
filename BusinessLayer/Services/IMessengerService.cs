using BusinessLayer.Dto;

namespace BusinessLayer.Services.Implementations;

public interface IMessengerService
{
    Task<MessengerDto> CreateMobilePhoneMessenger(Guid accountId, int phoneNumber, CancellationToken cancellationToken);
    Task<MessengerDto> CreateEmailMessenger(Guid accountId, string name, CancellationToken cancellationToken);
    Task<List<AbstractMessageDto>> LoadMessages(Guid employeeId, CancellationToken cancellationToken);
    Task<AbstractMessageDto> ReadMessage(Guid employeeId, Guid messageId, CancellationToken cancellationToken);
}