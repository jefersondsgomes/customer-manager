using CustomerManager.Model.Result;
using CustomerManager.Model.Transient;
using CustomerManager.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CustomerManager.Api.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("/api/v1/authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateRequest authenticateRequest)
        {
            var authenticationResult = await _authenticationService.AuthenticateAsync(authenticateRequest);
            if (authenticationResult.Error != null)
                return new ProblemDetailsResult(authenticationResult.StatusCode, Request, authenticationResult.Error).GetObjectResult();

            return Ok(authenticationResult.Value);
        }
    }
}