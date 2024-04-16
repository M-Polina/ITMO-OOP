using BusinessLayer.Dto;

namespace BusinessLayer.Services.Implementations;

public interface IReportService
{
   Task<ReportDto> CreateReport(CancellationToken cancellationToken);
    Task<List<ReportDto>> GetReportsByDate(DateTime time, CancellationToken cancellationToken);
    Task<List<ReportDto>> GetReports(CancellationToken cancellationToken);
}