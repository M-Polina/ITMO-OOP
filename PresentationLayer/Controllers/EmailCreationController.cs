using BusinessLayer.Dto;
using BusinessLayer.Mapping;
using BusinessLayer.Services;
using BusinessLayer.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailCreationController : ControllerBase
{
    private readonly IMessengerService _service;

    public EmailCreationController(IMessengerService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost]
    public async Task<MessengerDto> CreateEmailMesenger([FromBody] CreateEmailModel model)
    {
        var messenger = await _service.CreateEmailMessenger(model.AccountId,  model.Name, CancellationToken);
        return messenger;
    }
}