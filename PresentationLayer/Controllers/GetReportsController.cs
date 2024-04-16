using BusinessLayer.Dto;
using BusinessLayer.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Constants;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PolicyName.LeaderPolicy)]
public class GetReportsController : ControllerBase
{
    private readonly IReportService _service;

    public GetReportsController(IReportService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpGet]
    public async Task<List<ReportDto>> GetReports()
    {
        var report = await _service.GetReports(CancellationToken);
        return report;
    }
}