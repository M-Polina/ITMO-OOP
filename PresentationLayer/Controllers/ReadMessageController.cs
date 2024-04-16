using System.Security.Claims;
using BusinessLayer.Dto;
using BusinessLayer.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Constants;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PolicyName.OdrinaryEmployeePolicy)]
public class ReadMessageController : ControllerBase
{
    private readonly IMessengerService _service;

    public ReadMessageController(IMessengerService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost]
    public async Task<AbstractMessageDto> ReadMessage([FromBody] ReadMessageModel model)
    {
        Claim roleClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);

        var answer = await _service.ReadMessage(Guid.Parse(roleClaim.Value), model.MessageId, CancellationToken);
        return answer;
    }
}