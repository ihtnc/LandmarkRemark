using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LandmarkRemark.Api.Models;
using Xunit;
using FluentAssertions;

namespace LandmarkRemark.Api.Tests.Models
{
    public class ApiResponseHelperTests
    {
        [Fact]
        public void Ok_Should_Return_Correctly()
        {
            var message = "message";
            var actual = ApiResponseHelper.Ok(message);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
        }

        [Fact]
        public void Ok_Should_Return_Correctly_With_Data()
        {
            var content = "content";
            var message = "message";
            var actual = ApiResponseHelper.Ok(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content);
        }

        [Fact]
        public void Fail_Should_Return_Correctly()
        {
           var message = "message";
            var actual = ApiResponseHelper.Fail(message);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
        }

        [Fact]
        public void Fail_Should_Return_Correctly_With_Data()
        {
            var content = "content";
            var message = "message";
            var actual = ApiResponseHelper.Fail(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content);
        }

        [Fact]
        public void Created_Should_Return_Correctly()
        {
            var message = "message";
            var actual = ApiResponseHelper.Created(message);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status201Created);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
        }

        [Fact]
        public void Created_Should_Return_Correctly_With_Data()
        {
            var content = "content";
            var message = "message";
            var actual = ApiResponseHelper.Created(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status201Created);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content);
        }

        [Fact]
        public void BadRequest_Should_Return_Correctly()
        {
            var message = "message";
            var actual = ApiResponseHelper.BadRequest(message);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
        }

        [Fact]
        public void BadRequest_Should_Return_Correctly_With_Data()
        {
            var content = "content";
            var message = "message";
            var actual = ApiResponseHelper.BadRequest(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content);
        }

        [Fact]
        public void Unauthorised_Should_Return_Correctly()
        {
            var message = "message";
            var actual = ApiResponseHelper.Unauthorised(message);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
        }

        [Fact]
        public void Unauthorised_Should_Return_Correctly_With_Data()
        {
            var content = "content";
            var message = "message";
            var actual = ApiResponseHelper.Unauthorised(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status401Unauthorized);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content);
        }

        [Fact]
        public void Forbidden_Should_Return_Correctly()
        {
            var message = "message";
            var actual = ApiResponseHelper.Forbidden(message);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status403Forbidden);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
        }

        [Fact]
        public void Forbidden_Should_Return_Correctly_With_Data()
        {
            var content = "content";
            var message = "message";
            var actual = ApiResponseHelper.Forbidden(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status403Forbidden);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content);
        }

        [Fact]
        public void Error_Should_Return_Correctly()
        {
            var message = "message";
            var actual = ApiResponseHelper.Error(message);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
        }

        [Fact]
        public void Error_Should_Return_Correctly_With_Data()
        {
            var content = "content";
            var message = "message";
            var actual = ApiResponseHelper.Error(message, content);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(false);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be(message);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(content);
        }

        [Fact]
        public void HandleObjectResponse_Should_Return_Correctly()
        {
            var actual = ApiResponseHelper.ObjectResponse(StatusCodes.Status100Continue, true, message: "message", content: 123);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status100Continue);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("message");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(123);
        }
    }
}
