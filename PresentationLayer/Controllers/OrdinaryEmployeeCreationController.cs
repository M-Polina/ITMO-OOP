using System.Security.Claims;
using BusinessLayer.Dto;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Constants;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PolicyName.LeaderPolicy)]
public class OrdinaryEmployeeCreationController : ControllerBase
{
    private readonly IEmployeeService _service;

    public OrdinaryEmployeeCreationController(IEmployeeService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost]
    public async Task<OrdinaryEmployeeDto> Create([FromBody] CreateOrdinaryEmployeeModel model)
    {
        Claim roleClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);

        var employee = await _service.CreateOrdinaryEmployee(Guid.Parse(roleClaim.Value), model.Name, model.Login, model.Password, model.AccessLevel,  CancellationToken);

        return employee;
    }
}

