using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LandmarkRemark.Api.Models;
using LandmarkRemark.Api.Services;
using LandmarkRemark.Api.Security;

namespace LandmarkRemark.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemarksController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetRemarks([FromServices] IRemarksService remarksService)
        {
            var response = await remarksService.GetRemarks();
            return ApiResponseHelper.Ok($"{response.Count()} remark(s) found.", response);
        }

        [HttpPost]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> AddRemark([FromBody] AddRemarkRequest remark, [FromServices] IRemarksService remarksService, [FromServices] IUserDetailsProvider userDetailsProvider)
        {
            var userDetails = userDetailsProvider.GetUserDetails();
            var response = await remarksService.AddRemark(userDetails.Email, remark);
            return ApiResponseHelper.Created("Remark created.", response);
        }

        [HttpPatch("{remarkId}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> UpdateRemark(string remarkId, [FromBody] UpdateRemarkRequest updates, [FromServices] IRemarksService remarksService)
        {
            await remarksService.UpdateRemark(remarkId, updates);
            return ApiResponseHelper.Ok("Remark updated.", remarkId);
        }

        [HttpDelete("{remarkId}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> DeleteRemark(string remarkId, [FromServices] IRemarksService remarksService)
        {
            await remarksService.DeleteRemark(remarkId);
            return ApiResponseHelper.Ok("Remark deleted.", remarkId);
        }
    }
}
