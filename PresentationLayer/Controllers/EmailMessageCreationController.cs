using System.Security.Claims;
using BusinessLayer.Dto;
using BusinessLayer.Mapping;
using BusinessLayer.Services;
using BusinessLayer.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailMessageController : ControllerBase
{
    private readonly IMessageService _service;

    public EmailMessageController(IMessageService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost]
    public async Task<EmailMessageDto> CreateEmailMesage([FromBody] CreateEmailMessageModel model)
    {
        Claim roleClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
        if (roleClaim is not null)
        {
            var x = roleClaim.Value;
        }

        var messenger = await _service.CreateEmailMessage(model.MessengerId, 
            model.Theme, model.Message, CancellationToken);
        return messenger;
    }
}