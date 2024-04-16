using BusinessLayer.Dto;
using BusinessLayer.Mapping;
using BusinessLayer.Services;
using BusinessLayer.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhoneMessageController : ControllerBase
{
    private readonly IMessageService _service;

    public PhoneMessageController(IMessageService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost]
    public async Task<PhoneMessageDto> CreateMobilePhoneMessage([FromBody] CreatePhoneMessageModel model)
    {
        var messenger = await _service.CreateMobilePhoneMessage(model.MessengerId, model.Message, CancellationToken);
        return messenger;
    }
}