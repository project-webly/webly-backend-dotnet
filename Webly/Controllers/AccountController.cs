using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Webly.Configurations;
using Webly.Dtos;
using Webly.Models.Entity;

namespace Webly.Controllers;

[Route("/api/v1/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AccountEntity> _userManager;
    private readonly SignInManager<AccountEntity> _signInManager;
    private readonly AppConfig _appConfig;

    public AccountController(UserManager<AccountEntity> userManager, SignInManager<AccountEntity> signInManager, AppConfig appConfig)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _appConfig = appConfig;
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
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
	        return Unauthorized();
		}

        var claims = new List<Claim>
        {
	        new Claim(ClaimTypes.NameIdentifier, user.Id),
	        new Claim(ClaimTypes.Name, user.UserName)
        };


        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(
	        issuer: _appConfig.ValidIssuer,
	        audience: _appConfig.ValidAudience,
	        claims: claims,
	        expires: DateTime.Now.AddMinutes(720),
	        signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return Ok(new
        {
            AccessToken = jwt
        });
    }
}