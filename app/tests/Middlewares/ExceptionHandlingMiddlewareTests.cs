using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LandmarkRemark.Api.Middlewares;
using LandmarkRemark.Api.Models;
using Xunit;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace LandmarkRemark.Api.Tests.Middlewares
{
    public class ExceptionHandlingMiddlewareTests
    {
        private readonly IDummyRequestDelegate _innerFunction;
        private readonly RequestDelegate _next;
        private readonly ExceptionHandlingMiddleware _middleware;

        public ExceptionHandlingMiddlewareTests()
        {
            _innerFunction = Substitute.For<IDummyRequestDelegate>();
            _next = async (context) =>
            {
                await _innerFunction.CallMe(context);
                await Task.CompletedTask;
            };

           _middleware = Substitute.ForPartsOf<ExceptionHandlingMiddleware>(_next);
        }

        [Fact]
        public async void InvokeAsync_Should_Call_Next_Middleware()
        {
            var context = new DefaultHttpContext();

            await _middleware.InvokeAsync(context);

            await _innerFunction.Received(1).CallMe(context);
        }

        [Fact]
        public async void InvokeAsync_Should_Call_HandleException_When_An_Error_Occurred()
        {
            _middleware.When(x => x.HandleException(Arg.Any<Exception>(), Arg.Any<HttpContext>())).DoNotCallBase();

            var exception = new Exception();
            _innerFunction.CallMe(Arg.Any<HttpContext>()).Throws(exception);

            var context = new DefaultHttpContext();
            await _middleware.InvokeAsync(context);

            await _middleware.Received(1).HandleException(exception, context);
        }

        [Fact]
        public async void HandleException_Should_Set_StatusCode()
        {
            var exception = new Exception();
            var context = new DefaultHttpContext();

            await _middleware.HandleException(exception, context);

            context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async void HandleException_Should_Set_ContentType()
        {
            var exception = new Exception();
            var context = new DefaultHttpContext();

            await _middleware.HandleException(exception, context);

            context.Response.ContentType.Should().Be("application/json");
        }

        [Fact]
        public async void HandleException_Should_Set_Response_Body_With_DetailedResponse()
        {
            var exception = new Exception("ExceptionMessage");
            var context = new DefaultHttpContext();

            context.Response.Body = new MemoryStream();
            await _middleware.HandleException(exception, context);

            JToken body;
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using(var reader = new StreamReader(context.Response.Body))
            {
                var content = await reader.ReadToEndAsync();
                body = JToken.Parse(content);
            }

            var expected = ApiResponseHelper.Error("An unhandled exception has occurred.", exception.Message);
            var result = (ObjectResult) expected.Result;

            var expectedBody = JToken.FromObject(result.Value, new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            body.Should().BeEquivalentTo(expectedBody);
        }

        public interface IDummyRequestDelegate { Task CallMe(HttpContext context); }
    }
}