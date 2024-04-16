using BusinessLayer.Dto;
using BusinessLayer.Mapping;
using BusinessLayer.Services;
using BusinessLayer.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MobilePhoneCreationController : ControllerBase
{
    private readonly IMessengerService _service;

    public MobilePhoneCreationController(IMessengerService service)
    {
        _service = service;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost]
    public async Task<MessengerDto> CreateMobilePhoneMessenger([FromBody] CreateMobilePhoneModel model)
    {
        var messenger = await _service.CreateMobilePhoneMessenger(model.AccountId, model.PhoneNumber, CancellationToken);

        return messenger;
    }
}
    
