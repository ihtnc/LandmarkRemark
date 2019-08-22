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
            var response = await remarksService.AddRemark(userDetails.UserId, remark);
            return ApiResponseHelper.Created("Remark created.", response);
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
