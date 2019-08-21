using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LandmarkRemark.Api.Models;

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
        public async Task<ActionResult<ApiResponse>> AddRemark([FromBody] AddRemarkRequest remark)
        {
            var created = ApiResponseHelper.Created("Remark created.", "remarkId");
            return await Task.FromResult(created);
        }
    }
}
