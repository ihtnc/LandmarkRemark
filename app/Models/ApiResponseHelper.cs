using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LandmarkRemark.Api.Models
{
    public static class ApiResponseHelper
    {
        public static ActionResult<ApiResponse> Ok(string message)
        {
            return Ok(message,  default(object));
        }

        public static ActionResult<ApiResponse> Ok<T>(string message, T data)
        {
            return ObjectResponse(StatusCodes.Status200OK, true, message: message, content: data);
        }

        public static ActionResult<ApiResponse> Fail(string message)
        {
            return Fail(message, default(object));
        }

        public static ActionResult<ApiResponse> Fail<T>(string message, T data)
        {
            return ObjectResponse(StatusCodes.Status200OK, false, message: message, content: data);
        }

        public static ActionResult<ApiResponse> Created(string message)
        {
            return Created(message, default(object));
        }

        public static ActionResult<ApiResponse> Created<T>(string message, T data)
        {
            return ObjectResponse(StatusCodes.Status201Created, true, message: message, content: data);
        }

        public static ActionResult<ApiResponse> BadRequest(string message)
        {
            return BadRequest(message, default(object));
        }

        public static ActionResult<ApiResponse> BadRequest<T>(string message, T data)
        {
            return ObjectResponse(StatusCodes.Status400BadRequest, false, message: message, content: data);
        }

        public static ActionResult<ApiResponse> Unauthorised(string message)
        {
            return Unauthorised(message, default(object));
        }

        public static ActionResult<ApiResponse> Unauthorised<T>(string message, T data)
        {
            return ObjectResponse(StatusCodes.Status401Unauthorized, false, message: message, content: data);
        }

        public static ActionResult<ApiResponse> Forbidden(string message)
        {
            return Forbidden(message, default(object));
        }

         public static ActionResult<ApiResponse> Forbidden<T>(string message, T data)
        {
            return ObjectResponse(StatusCodes.Status403Forbidden, false, message: message, content: data);
        }

        public static ActionResult<ApiResponse> Error(string message)
        {
            return Error(message, default(object));
        }

        public static ActionResult<ApiResponse> Error<T>(string message, T data)
        {
            return ObjectResponse(StatusCodes.Status500InternalServerError, false, message: message, content: data);
        }

        public static ActionResult<ApiResponse> ObjectResponse<T>(int statusCode, bool success, string message, T content = default)
        {
            var response = new ApiResponse
            {
                Success = success,
                Message = message
            };

            if (content != default) { response.Data = content; }

            return new ObjectResult(response) { StatusCode = statusCode };
        }
    }
}
