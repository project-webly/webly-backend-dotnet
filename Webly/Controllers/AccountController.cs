using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webly.Dtos;
using Webly.Models.Entity;

namespace Webly.Controllers;

[Route("/api/v1/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AccountEntity> _userManager;
    private readonly SignInManager<AccountEntity> _signInManager;

    public AccountController(UserManager<AccountEntity> userManager, SignInManager<AccountEntity> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult GetAccount()
    {
        return Ok(new AccountDto()
        {
            UserName = User.Identity.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new AccountEntity()
        {
            UserName = dto.UserName
        };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, false, false);
        if (result.Succeeded)
            return Ok();

        return Unauthorized();
    }
}