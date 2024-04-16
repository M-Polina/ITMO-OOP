using BusinessLayer.Dto;
using BusinessLayer.Mapping;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Implementations.ReportService;

public class ReportService : IReportService
{
    private const int MinNumber = 0;
    private readonly DatabaseContext _context;

    public ReportService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ReportDto> CreateReport(CancellationToken cancellationToken)
    {
        int allMessagesStatistic = 0;
        var messengerStatistic = new List<MessengerStatistic>();
        var employeeStatistic = new List<EmployeeStatistic>();

        var messages = await _context.Messages.ToListAsync(cancellationToken);
        allMessagesStatistic = messages.Where(m => m.Status != MessageStatus.ProcessedMessage).ToList().Count;

        var tasks = await _context.EmployeeTasks.ToListAsync(cancellationToken);

        foreach (var task in tasks)
        {
            Console.WriteLine(task.Status.ToString("G"));
            if (task.Status == EmployeeTaskStatus.NewTask)
            {
                allMessagesStatistic++;
                MessengerStatistic mesStat = messengerStatistic.Find(m => m.MessengerId == task.MessengerId);
                if (mesStat is null)
                {
                    mesStat = new MessengerStatistic( Guid.NewGuid(), task.MessengerId, MinNumber);
                   messengerStatistic.Add(mesStat);
                }

                mesStat.Number++;

                EmployeeStatistic empStat = employeeStatistic.Find(m => m.EmployeeId == task.EmployeeId);
                if (empStat is null)
                {
                    empStat = new EmployeeStatistic(Guid.NewGuid(), task.EmployeeId, MinNumber);
                    employeeStatistic.Add(empStat);
                }

                empStat.Number++;

                task.Status = EmployeeTaskStatus.ProcessedTask;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        var report = new Report(Guid.NewGuid(), allMessagesStatistic, messengerStatistic, employeeStatistic,
            DateTime.Now);

        await _context.Reports.AddAsync(report, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return report.AsDto();
    }

    public async Task<List<ReportDto>> GetReportsByDate(DateTime time, CancellationToken cancellationToken)
    {
        var reports = await _context.Reports.ToListAsync(cancellationToken);

        return reports.Where(r => r.CreationDate.Equals(time)).Select(r => r.AsDto()).ToList();
    }

    public async Task<List<ReportDto>> GetReports(CancellationToken cancellationToken)
    {

        var reports = await _context.Reports.ToListAsync(cancellationToken);
        return reports.Select(r => r.AsDto()).ToList();
    }
}