using CustomerManager.Model.Common;
using CustomerManager.Model.Result;
using CustomerManager.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CustomerManager.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("/api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>        
        /// <param name="user">User model</param>
        /// <returns>A newly created user</returns>
        /// <response code="201">A newly created user</response>
        /// <response code="400">If the user is null</response>        
        /// <response code="409">If user already exists</response>
        /// <response code="500">When application fail</response>                             
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync(User user)
        {
            var userResult = await _userService.CreateAsync(user);
            if (userResult.Error != null)
                return new ProblemDetailsResult(userResult.StatusCode, Request, userResult.Error).GetObjectResult();

            return CreatedAtRoute(new { id = user.Id }, user);
        }
    }
}