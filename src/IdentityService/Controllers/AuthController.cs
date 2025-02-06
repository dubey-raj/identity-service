using IdentityService.Model;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthController(ILogger<AuthController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("token")]
        public IActionResult GenerateToken([FromBody] LoginRequest request)
        {
            if (request.GrantType == AppConstants.GrantType_Password)
            {
                var user = _authenticationService.ValidateUser(request.UserName, request.Password);

                if (user.IsValid)
                {
                    var token = _authenticationService.CreateToken(user.UserInfo);
                    return Ok(new { AccessToken = token });
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else if(request.GrantType == AppConstants.GrantType_ClientCredential)
            {
                var client = _authenticationService.ValidateClient(request.ClientId, request.ClientSecret);
                if (client.IsValid)
                {
                    var token = _authenticationService.CreateToken(client.ClientInfo);
                    return Ok(new { AccessToken = token });
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }

            return BadRequest("Invalid request");
        }
    }
}
