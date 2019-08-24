using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LandmarkRemark.Api.Models;
using LandmarkRemark.Api.Services;

namespace LandmarkRemark.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("Register")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] SecurityRequest request, [FromServices] ISecurityService securityService)
        {
            var response = await securityService.Register(request);
            return ApiResponseHelper.Created("User created.", response);
        }

        [HttpPost("LogIn")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> LogIn([FromBody] SecurityRequest request, [FromServices] ISecurityService securityService)
        {
            var response = await securityService.LogIn(request);
            return ApiResponseHelper.Ok("User logged in.", response);
        }
    }
}
