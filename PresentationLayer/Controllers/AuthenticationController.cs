using System.Net;
using System.Security.Claims;
using BusinessLayer.Dto;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IEmployeeService _accountService;

    public AuthenticationController(IEmployeeService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("LogIn")]
    public async Task<IActionResult> LogIn(string username, string password)
    {
        AbstractEmployeeDto employee = await _accountService.FindAccount(username, password);

        if (employee is null)
        {
            return AccessDenied();
        }

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Name, employee.Name),
            new Claim(ClaimTypes.Sid, employee.Id.ToString("D")),
            new Claim(ClaimTypes.Role, employee.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return Ok(employee);
    }

    [HttpPost("SignOut")]
    public async Task<string> LogOut()
    {
        await HttpContext.SignOutAsync();
        return "Signed out.";
    }
    
    [Route("Error")]
    [HttpGet]
    [HttpPost]
    public IActionResult AccessDenied()
    {
        Claim roleClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
        return roleClaim is not null
            ? this.StatusCode((int)HttpStatusCode.Forbidden, $"User with necessary role is not authorized to invoke this method. Current role: {roleClaim.Value}")
            : this.Unauthorized("User is not authenticated");
    }
}