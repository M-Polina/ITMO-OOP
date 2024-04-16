using System.Security.Claims;
using BusinessLayer.Dto;
using BusinessLayer.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Constants;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PolicyName.OdrinaryEmployeePolicy)]
public class LoadMessagesController : ControllerBase
{
    private readonly IMessengerService _service;

    public LoadMessagesController(IMessengerService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpGet]
    public async Task<List<AbstractMessageDto>> LoadMessages()
    {
        Claim roleClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
        
        Console.WriteLine(Guid.Parse(roleClaim.Value));

        var answer = await _service.LoadMessages(Guid.Parse(roleClaim.Value), CancellationToken);
        return answer;
    }
}