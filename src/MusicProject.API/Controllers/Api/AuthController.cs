using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicProject.Entities.Content;
using MusicProject.Entities.Identity;
using MusicProject.Services.Contracts.Content;
using MusicProject.ViewModels.Api;

namespace MusicProject.API.Controllers.Api
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IConfiguration _config;
    private readonly IDeviceService _deviceService;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AuthController(
      IConfiguration config,
      IDeviceService deviceService,
      UserManager<User> userManager,
      SignInManager<User> signInManager
      )
    {
      _config = config;
      _deviceService = deviceService;
      _userManager = userManager;
      _signInManager = signInManager;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> GenerateToken([FromBody] LoginViewModel model)
    {
      var exp = DateTime.UtcNow.Add(TimeSpan.FromDays(10));
      var device = _deviceService.FindByDeviceAsync(model.DeviceId).Result ?? _deviceService.AddNewDevice(new Device
      {
        DeviceId = model.DeviceId,
        UserId = null
      });

      var claims = new[]
       {
          new Claim(JwtRegisteredClaimNames.Sub, device.Id.ToString()),

          new Claim(JwtRegisteredClaimNames.Exp, exp.ToString()),
          new Claim("deviceId",device.Id.ToString()),
          new Claim("type", "device"),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
          new Claim(JwtRegisteredClaimNames.Iat,  DateTime.UtcNow.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
        }.ToList();


      var user = await _userManager.FindByNameAsync(model.Username);

      if (user != null)
      {
        var result = await _signInManager.PasswordSignInAsync(
          model.Username,
          model.Password,
         false,
          true);
        if (result.Succeeded)
        {
          claims.Add(new Claim(ClaimTypes.Name, user.UserName));
          claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
          claims.Add(new Claim(ClaimTypes.Email, user.Email));
        }
      }
      
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var token = new JwtSecurityToken(_config["Tokens:Issuer"],
        _config["Tokens:Issuer"],
        claims.ToList(),
        expires: exp,
        signingCredentials: creds
      );
      return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
  }
}