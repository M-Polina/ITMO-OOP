using BusinessLayer.Services;
using BusinessLayer.Services.Implementations;
using BusinessLayer.Services.Implementations.AccountService;
using BusinessLayer.Services.Implementations.EmployeeService;
using BusinessLayer.Services.Implementations.MessageService;
using BusinessLayer.Services.Implementations.MessengerService;
using BusinessLayer.Services.Implementations.ReportService;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IAccountService, AccountService>();
        collection.AddScoped<IMessengerService, MessengerService>();
        collection.AddScoped<IMessageService, MessageService>();
        collection.AddScoped<IEmployeeService, EmployeeService>();
        collection.AddScoped<IReportService, ReportService>();

        return collection;
    }
}