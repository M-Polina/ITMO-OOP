using BusinessLayer.Dto;
using BusinessLayer.Exceptions;
using BusinessLayer.Extensions;
using BusinessLayer.Mapping;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Implementations;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Implementations.MessengerService;

public class MessengerService : IMessengerService
{
    private const int MinNumber = 0;
    private readonly DatabaseContext _context;

    public MessengerService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<MessengerDto> CreateEmailMessenger(Guid accountId, string name,
        CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.GetEntityAsync(accountId, cancellationToken);

        if (account is null)
            throw new BusinessLayerException("Null account while creating messenger");
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessLayerException("IsNullOrWhiteSpace name while creating messenger");

        var messenger = new Email(Guid.NewGuid(), account, name);

        account.Messengers.Add(messenger);
        await _context.Messengers.AddAsync(messenger);
        await _context.SaveChangesAsync(cancellationToken);

        return messenger.AsDto();
    }

    public async Task<MessengerDto> CreateMobilePhoneMessenger(Guid accountId, int phoneNumber,
        CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.GetEntityAsync(accountId, cancellationToken);

        if (phoneNumber <= MinNumber)
            throw new BusinessLayerException("PhoneNumber <= 0 while creating messenger");

        if (account is null)
            throw new BusinessLayerException("Null Account while creating messenger");

        var messenger = new MobilePhone(Guid.NewGuid(), account, phoneNumber);

        account.Messengers.Add(messenger);
        await _context.Messengers.AddAsync(messenger);
        await _context.SaveChangesAsync(cancellationToken);

        return messenger.AsDto();
    }

    public async Task<List<AbstractMessageDto>> LoadMessages(Guid employeeId, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.SingleOrDefaultAsync(em => em.Id.Equals(employeeId));

        if (employee is null)
            throw new BusinessLayerException("Not signed in while loadding messages.");

        var messages = new List<AbstractMessage>();

        var accounts = await _context.Accounts.ToListAsync();
        int acceccLevel = employee.AccessLevel;
        List<Account> availableAccounts = accounts.Where(ac => ac.MinAccessLevel >= acceccLevel).ToList();
        foreach (var acc in availableAccounts)
        {
            foreach (var messenger in acc.Messengers)
            {
                foreach (var message in messenger.Messages)
                {
                    if (message.Status == MessageStatus.NewMessage)
                    {
                        message.Status = MessageStatus.RecievedMessage;
                        messages.Add(message);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }
            }
        }

        if (messages.Count == MinNumber)
            return new List<AbstractMessageDto>();

        return messages.Select(m => m.AsDto()).ToList();
    }


    public async Task<AbstractMessageDto> ReadMessage(Guid employeeId, Guid messageId, CancellationToken cancellationToken)
    {
            
        var employee = await _context.Employees.SingleOrDefaultAsync(em => em.Id.Equals(employeeId));

        if (employee is null)
            throw new BusinessLayerException("Not signed in while reading message.");

        if (employee is Leader)
            throw new BusinessLayerException("Leader mustn't read messeges.");

        var message = await _context.Messages.FindAsync(messageId);

        if (message is null)
            throw new BusinessLayerException("Messange not found.");

        if (message.Messenger.Account.MinAccessLevel < employee.AccessLevel)
            throw new BusinessLayerException("Not enought access level.");

        if (message.Status != MessageStatus.RecievedMessage)
            throw new BusinessLayerException("Message is unloaded.");

        if (message.Status == MessageStatus.ProcessedMessage)
        {
            return message.AsDto();
        }

        var task = new EmployeeTask(Guid.NewGuid(), employee.Id ,message.Id, message.Messenger.Id, DateTime.Now, employee);

        message.Status = MessageStatus.ProcessedMessage;
        await _context.SaveChangesAsync(cancellationToken);

        (employee as OrdinaryEmployee).Tasks.Add(task);
        await _context.EmployeeTasks.AddAsync(task);
        await _context.SaveChangesAsync(cancellationToken);


        return message.AsDto();
    }
}