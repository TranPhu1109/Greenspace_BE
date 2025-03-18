using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Application.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Endpoint Đăng nhập, gửi firebasetoken, roleName
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequest)
    {
        var result = await _authService.LoginAsync(loginRequest.Token,
            loginRequest.FCMToken,
            loginRequest.Role ?? string.Empty);
        if (result is not null) return Ok(result);
        else return BadRequest("Login Failed");
    }
    /// <summary>
    /// Endpoint Refresh lại token
    /// </summary>
    /// <param name="token">Old Token</param>
    /// <returns></returns>
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string token)
    {
        var result = await _authService.RefreshTokenAsync(token);
        return Ok(result);
    }
}
