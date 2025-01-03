using IdentityService.Model;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public IdentityController(ILogger<IdentityController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Mode == RequestMode.Password)
            {
                var user = _authenticationService.ValidateUser(request.UserName, request.Password);

                if (user.IsValid)
                {
                    var token = _authenticationService.GenerateToken(user.UserInfo);
                    return Ok(new { AccessToken = token });
                }
            }
            else if(request.Mode == RequestMode.ClientCredential)
            {

            }

            return BadRequest();
        }
    }
}
