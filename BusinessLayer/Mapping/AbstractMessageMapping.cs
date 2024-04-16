using BusinessLayer.Dto;
using DataAccessLayer.Models.Implementations;

namespace BusinessLayer.Mapping;

public static class AbstractMessageMapping
{
    public static AbstractMessageDto AsDto(this AbstractMessage message)
        => new AbstractMessageDto(message.Id, message.Status.ToString(), 
            (message is EmailMessage)? (message as EmailMessage).Theme : "",
            message.StringMessage);
}
