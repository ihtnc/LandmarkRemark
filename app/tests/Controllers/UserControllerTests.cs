using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LandmarkRemark.Api.Controllers;
using LandmarkRemark.Api.Models;
using LandmarkRemark.Api.Services;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace LandmarkRemark.Api.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _controller = new UserController();
        }

        [Fact]
        public void Class_Should_Include_ApiControllerAttribute()
        {
            var t = _controller.GetType();
            t.Should().BeDecoratedWith<ApiControllerAttribute>();
        }

        [Fact]
        public void Class_Should_Include_RouteAttribute()
        {
            var t = _controller.GetType();
            t.Should().BeDecoratedWith<RouteAttribute>(attr => attr.Template == "api/[controller]");
        }

        [Fact]
        public void Register_Should_Include_HttpPostAttribute()
        {
            var methodName = nameof(_controller.Register);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<HttpPostAttribute>()
                .Which.Template.Should().Be("Register");
        }

        [Fact]
        public void Register_Should_Include_ProducesAttribute()
        {
            var methodName = nameof(_controller.Register);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<ProducesAttribute>()
                .Which.ContentTypes.Should().Contain("application/json");
        }

        [Theory]
        [InlineData(StatusCodes.Status201Created, null)]
        [InlineData(StatusCodes.Status400BadRequest, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status500InternalServerError, typeof(ApiResponse))]
        public void Register_Should_Include_ProducesResponseTypeAttribute(int statusCode, Type responseType)
        {
            var methodName = nameof(_controller.Register);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<ProducesResponseTypeAttribute>(attr => attr.StatusCode == statusCode && (responseType == null || attr.Type == responseType));
        }

        [Fact]
        public async void Register_Should_Call_ISecurityService_Register()
        {
            var request = new SecurityRequest();
            var service = Substitute.For<ISecurityService>();

            await _controller.Register(request, service);

            await service.Received(1).Register(request);
        }

        [Fact]
        public async void Register_Should_Return_Correctly()
        {
            var response = new SecurityResponse();
            var service = Substitute.For<ISecurityService>();
            service.Register(Arg.Any<SecurityRequest>()).Returns(response);

            var actual = await _controller.Register(new SecurityRequest(), service);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status201Created);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("User created.");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(response);
        }

        [Fact]
        public void LogIn_Should_Include_HttpPostAttribute()
        {
            var methodName = nameof(_controller.LogIn);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<HttpPostAttribute>()
                .Which.Template.Should().Be("LogIn");
        }

        [Fact]
        public void LogIn_Should_Include_ProducesAttribute()
        {
            var methodName = nameof(_controller.LogIn);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<ProducesAttribute>()
                .Which.ContentTypes.Should().Contain("application/json");
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK, null)]
        [InlineData(StatusCodes.Status400BadRequest, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status500InternalServerError, typeof(ApiResponse))]
        public void LogIn_Should_Include_ProducesResponseTypeAttribute(int statusCode, Type responseType)
        {
            var methodName = nameof(_controller.LogIn);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<ProducesResponseTypeAttribute>(attr => attr.StatusCode == statusCode && (responseType == null || attr.Type == responseType));
        }

        [Fact]
        public async void LogIn_Should_Call_ISecurityService_LogIn()
        {
            var request = new SecurityRequest();
            var service = Substitute.For<ISecurityService>();

            await _controller.LogIn(request, service);

            await service.Received(1).LogIn(request);
        }

        [Fact]
        public async void LogIn_Should_Return_Correctly()
        {
            var response = new SecurityResponse();
            var service = Substitute.For<ISecurityService>();
            service.LogIn(Arg.Any<SecurityRequest>()).Returns(response);

            var actual = await _controller.LogIn(new SecurityRequest(), service);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("User logged in.");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(response);
        }
    }
}
