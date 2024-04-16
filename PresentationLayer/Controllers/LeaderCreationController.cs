using BusinessLayer.Dto;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderCreationController : ControllerBase
{
    private readonly IEmployeeService _service;

    public LeaderCreationController(IEmployeeService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost]
    public async Task<LeaderDto> CreateLeaderAccount([FromBody] CreateLeaderModel model)
    {
        var employee = await _service.CreateLeaderAccount(model.Name, model.Login, model.Password, CancellationToken);

        return employee;
    }
}

