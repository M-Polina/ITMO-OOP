using BusinessLayer.Dto;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _service;

    public AccountController(IAccountService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost]
    public async Task<AccountDto> CreateAsync([FromBody] CreateAccountModel model)
    {
        var account = await _service.CreateAccount(model.MinAccessLevel, CancellationToken);
        return account;
    }
}