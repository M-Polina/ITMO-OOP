using BusinessLayer.Dto;
using BusinessLayer.Services;
using BusinessLayer.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Constants;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PolicyName.LeaderPolicy)]
public class ReportCreationController : ControllerBase
{
    private readonly IReportService _service;

    public ReportCreationController(IReportService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost]
    public async Task<ReportDto> CreateReport()
    {
        var report = await _service.CreateReport(CancellationToken);
        return report;
    }
}