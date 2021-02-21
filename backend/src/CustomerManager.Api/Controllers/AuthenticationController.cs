using CustomerManager.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManager.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }
    }
}
