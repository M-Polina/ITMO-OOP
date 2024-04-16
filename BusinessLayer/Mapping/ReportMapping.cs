using BusinessLayer.Dto;
using DataAccessLayer.Models;

namespace BusinessLayer.Mapping;

public static class ReportMapping
{
    public static ReportDto AsDto(this Report report)
        => new ReportDto(report.Id,
            report.MessengerStatistic,
            report.EmployeeStatistic,
            report.AllMessagesStatistic,
            report.CreationDate.ToString("MM.dd.yyyy-HH:mm:ss"));
}