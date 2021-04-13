using CustomerManager.Model.Common;
using CustomerManager.Model.Result;
using CustomerManager.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CustomerManager.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(User user)
        {
            var userResult = await _userService.CreateAsync(user);
            if (userResult.Error != null)
                return new ProblemDetailsResult(userResult.StatusCode, Request, userResult.Error).GetObjectResult();

            return CreatedAtRoute(new { id = user.Id }, user);
        }
    }
}