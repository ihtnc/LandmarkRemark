using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LandmarkRemark.Api.Controllers;
using LandmarkRemark.Api.Models;
using Xunit;
using FluentAssertions;

namespace LandmarkRemark.Api.Tests.Controllers
{
    public class RemarksControllerTests
    {
        private readonly RemarksController _controller;

        public RemarksControllerTests()
        {
            _controller = new RemarksController();
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
        public void AddRemark_Should_Include_HttpPostAttribute()
        {
            var methodName = nameof(_controller.AddRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<HttpPostAttribute>();
        }

        [Fact]
        public void AddRemark_Should_Include_AuthorizeAttribute()
        {
            var methodName = nameof(_controller.AddRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void GetUser_Should_Include_ProducesAttribute()
        {
            var methodName = nameof(_controller.AddRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<ProducesAttribute>().Which.ContentTypes.Should().Contain("application/json");
        }

        [Theory]
        [InlineData(StatusCodes.Status201Created, null)]
        [InlineData(StatusCodes.Status400BadRequest, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status401Unauthorized, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status403Forbidden, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status500InternalServerError, typeof(ApiResponse))]
        public void AddRemark_Should_Include_ProducesResponseTypeAttribute(int statusCode, Type responseType)
        {
            var methodName = nameof(_controller.AddRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<ProducesResponseTypeAttribute>(attr => attr.StatusCode == statusCode && (responseType == null || attr.Type == responseType));
        }
    }
}
