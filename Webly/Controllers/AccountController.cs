using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Webly.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [HttpGet]
    public IActionResult GetAccount()
    {
        if (User.Identity.IsAuthenticated)
        {
            return Ok(new AccountDto()
            {
                UserName = User.Identity.Name
            });
        }

        return Unauthorized();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new IdentityUser(dto.UserName);
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok();
        }

        return BadRequest();
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

public class AccountDto
{
    public string UserName { get; init; }
}

public class RegisterDto
{
    public string UserName { get; init; }
    
    public string Password { get; init; }
}

public class LoginDto
{
    public string UserName { get; init; }
    public string Password { get; init; }
}