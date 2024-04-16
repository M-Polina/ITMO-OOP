using BusinessLayer.Dto;
using DataAccessLayer.Models.Implementations;

namespace BusinessLayer.Mapping;

public static class EmailMessageMapping
{
    public static EmailMessageDto AsDto(this EmailMessage emailMessage)
        => new EmailMessageDto(emailMessage.Id, emailMessage.Status.ToString(), emailMessage.Theme, emailMessage.StringMessage);
}