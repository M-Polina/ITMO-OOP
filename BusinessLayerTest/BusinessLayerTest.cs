using BusinessLayer.Dto;
using BusinessLayer.Services.Implementations.AccountService;
using BusinessLayer.Services.Implementations.EmployeeService;
using BusinessLayer.Services.Implementations.MessageService;
using BusinessLayer.Services.Implementations.MessengerService;
using BusinessLayer.Services.Implementations.ReportService;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BusinessLayesTest;

public class BusinessLayerTest : IDisposable
{
    private readonly DatabaseContext _db;

    private EmployeeService EmployeeService;
    private AccountService AccountService;
    private MessageService MessageService;
    private MessengerService MessengerService;
    private ReportService ReportService;

    public BusinessLayerTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        DbContextOptions<DatabaseContext> options =
            optionsBuilder.UseInMemoryDatabase(databaseName: $"{Guid.NewGuid().ToString()}").Options;
        _db = new DatabaseContext(options);

        EmployeeService = new EmployeeService(_db);
        AccountService = new AccountService(_db);
        MessageService = new MessageService(_db);
        MessengerService = new MessengerService(_db);
        ReportService = new ReportService(_db);
    }

    [Fact]
    public void LoadMessages_MessageIsLoaded()
    {
        var leaderDto =  EmployeeService.CreateLeaderAccount("Den", "1", "1", CancellationToken.None);
        var accountDto = AccountService.CreateAccount(4, CancellationToken.None);
        var messengerDto = MessengerService.CreateMobilePhoneMessenger(accountDto.Result.Id, 12345, CancellationToken.None);
        var messageDto =
            MessageService.CreateMobilePhoneMessage(messengerDto.Result.Id, "Hello", CancellationToken.None);
        var employeeDto =
            EmployeeService.CreateOrdinaryEmployee(leaderDto.Result.Id, "Sue", "q", "q", 1, CancellationToken.None);
        var messagesDto = MessengerService.LoadMessages(employeeDto.Result.Id, CancellationToken.None);

        Assert.Equal(messagesDto.Result[0].Status, MessageStatus.RecievedMessage.ToString("G"));
    }
    
    [Fact]
    public void ReadMessage_MessageIsProseeded()
    {
        var leaderDto =  EmployeeService.CreateLeaderAccount("Den", "1", "1", CancellationToken.None);
        var accountDto = AccountService.CreateAccount(4, CancellationToken.None);
        var messengerDto = MessengerService.CreateMobilePhoneMessenger(accountDto.Result.Id, 12345, CancellationToken.None);
        var messageDto =
            MessageService.CreateMobilePhoneMessage(messengerDto.Result.Id, "Hello", CancellationToken.None);
        var employeeDto =
            EmployeeService.CreateOrdinaryEmployee(leaderDto.Result.Id, "Sue", "q", "q", 1, CancellationToken.None);
        var messagesDto = MessengerService.LoadMessages(employeeDto.Result.Id, CancellationToken.None);

        var readMessageDto = MessengerService.ReadMessage(employeeDto.Result.Id, messagesDto.Result[0].MessageId , CancellationToken.None);
        
        Assert.Equal(readMessageDto.Result.Status, MessageStatus.ProcessedMessage.ToString("G"));
    }
    
    [Fact]
    public void CreateReport_MessageIsInReport()
    {
        var leaderDto =  EmployeeService.CreateLeaderAccount("Den", "1", "1", CancellationToken.None);
        var accountDto = AccountService.CreateAccount(4, CancellationToken.None);
        var messengerDto = MessengerService.CreateMobilePhoneMessenger(accountDto.Result.Id, 12345, CancellationToken.None);
        var messageDto =
            MessageService.CreateMobilePhoneMessage(messengerDto.Result.Id, "Hello", CancellationToken.None);
        var employeeDto =
            EmployeeService.CreateOrdinaryEmployee(leaderDto.Result.Id, "Sue", "q", "q", 1, CancellationToken.None);
        var messagesDto = MessengerService.LoadMessages(employeeDto.Result.Id, CancellationToken.None);

        var readMessageDto = MessengerService.ReadMessage(employeeDto.Result.Id, messagesDto.Result[0].MessageId , CancellationToken.None);

        var reportDto = ReportService.CreateReport(CancellationToken.None);
        
        Assert.Equal(reportDto.Result.MessengerStatistic[0].Number, 1);
    }

    [Fact]
    public void CreateReport_AddToMessagesFromDifferentMessengers_MessengerStatisticHasToMessegersWithOneMessage()
    {
        var leaderDto =  EmployeeService.CreateLeaderAccount("Den", "1", "1", CancellationToken.None);
        var accountDto = AccountService.CreateAccount(4, CancellationToken.None);
        var messengerDto = MessengerService.CreateMobilePhoneMessenger(accountDto.Result.Id, 12345, CancellationToken.None);
        var messageDto =
            MessageService.CreateMobilePhoneMessage(messengerDto.Result.Id, "Hello", CancellationToken.None);
        var messengerDto2 = MessengerService.CreateEmailMessenger(accountDto.Result.Id, "gmail", CancellationToken.None);
        var messageDto2 =
            MessageService.CreateEmailMessage(messengerDto2.Result.Id, "Hello","I am back!", CancellationToken.None);
        var employeeDto =
            EmployeeService.CreateOrdinaryEmployee(leaderDto.Result.Id, "Sue", "q", "q", 1, CancellationToken.None);
        var messagesDto = MessengerService.LoadMessages(employeeDto.Result.Id, CancellationToken.None);

        var readMessageDto = MessengerService.ReadMessage(employeeDto.Result.Id, messagesDto.Result[0].MessageId , CancellationToken.None);
        var readMessageDto2 = MessengerService.ReadMessage(employeeDto.Result.Id, messagesDto.Result[1].MessageId , CancellationToken.None);

        var reportDto = ReportService.CreateReport(CancellationToken.None);
        
        Assert.Equal(reportDto.Result.MessengerStatistic[0].Number, 1);
        Assert.Equal(reportDto.Result.MessengerStatistic[1].Number, 1);
        Assert.Equal(reportDto.Result.MessengerStatistic.Count, 2);
    }
    public void Dispose() => _db.Dispose();
}