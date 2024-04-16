using System.Net;
using BusinessLayer.Dto;
using BusinessLayer.Exceptions;
using BusinessLayer.Extensions;
using BusinessLayer.Mapping;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Implementations;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Implementations.MessageService;

public class MessageService : IMessageService
{
    private readonly DatabaseContext _context;

    public MessageService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<EmailMessageDto> CreateEmailMessage(
        Guid messengerId,
        string theme,
        string message,
        CancellationToken cancellationToken)
    {
        var messenger = await _context.Messengers.GetEntityAsync(messengerId, cancellationToken);

        if (messenger is not Email)
            throw new BusinessLayerException("Trying to create not email message in email.");
        if (string.IsNullOrWhiteSpace(message))
            throw new BusinessLayerException("Null message while creating email message.");


        EmailMessage newMessage = new EmailMessage(Guid.NewGuid(), messenger, theme, message);
        messenger.Messages.Add(newMessage);
        await _context.Messages.AddAsync(newMessage);
        await _context.SaveChangesAsync(cancellationToken);

        return newMessage.AsDto();
    }

    public async Task<PhoneMessageDto> CreateMobilePhoneMessage(
        Guid messengerId,
        string message,
        CancellationToken cancellationToken)
    {
        var messenger = await _context.Messengers. FirstOrDefaultAsync( x => x.Id == messengerId, cancellationToken);

        if (messenger is not MobilePhone)
            throw new BusinessLayerException("Trying to create not MobilePhone message in MobilePhone.");
        if (string.IsNullOrWhiteSpace(message))
            throw new BusinessLayerException("Null message while creating phone message.");


        PhoneMessage newMessage = new PhoneMessage(Guid.NewGuid(), messenger, message);

        messenger.Messages.Add(newMessage);
        await _context.Messages.AddAsync(newMessage);
        await _context.SaveChangesAsync(cancellationToken);

        return newMessage.AsDto();
    }
}