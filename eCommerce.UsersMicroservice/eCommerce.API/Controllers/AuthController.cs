using eCommerce.Core.Dto;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUsersService usersService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest? registerRequest)
    {
        // Check for invalid registerRequest
        if (registerRequest == null)
            return BadRequest("Invalid registration data");

        // Call the UsersService to handle registration
        var authenticationResponse = await usersService.Register(registerRequest);

        if (authenticationResponse == null || authenticationResponse.Success == false)
            return BadRequest(authenticationResponse);

        return Ok(authenticationResponse);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest? loginRequest)
    {
        // Check for invalid LoginRequest
        if (loginRequest == null)
            return BadRequest("Invalid login data");

        var authenticationResponse = await usersService.Login(loginRequest);

        if (authenticationResponse == null || authenticationResponse.Success == false)
            return Unauthorized(authenticationResponse);

        return Ok(authenticationResponse);
    }
}
