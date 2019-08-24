using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LandmarkRemark.Api.Controllers;
using LandmarkRemark.Api.Models;
using LandmarkRemark.Api.Repositories.Models;
using LandmarkRemark.Api.Security;
using LandmarkRemark.Api.Services;
using Xunit;
using FluentAssertions;
using NSubstitute;

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
        public void GetRemarks_Should_Include_HttpGetAttribute()
        {
            var methodName = nameof(_controller.GetRemarks);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<HttpGetAttribute>();
        }

        [Fact]
        public void GetRemarks_Should_Include_AuthorizeAttribute()
        {
            var methodName = nameof(_controller.GetRemarks);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void GetRemarks_Should_Include_ProducesAttribute()
        {
            var methodName = nameof(_controller.GetRemarks);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<ProducesAttribute>()
                .Which.ContentTypes.Should().Contain("application/json");
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK, null)]
        [InlineData(StatusCodes.Status401Unauthorized, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status403Forbidden, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status500InternalServerError, typeof(ApiResponse))]
        public void GetRemarks_Should_Include_ProducesResponseTypeAttribute(int statusCode, Type responseType)
        {
            var methodName = nameof(_controller.GetRemarks);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<ProducesResponseTypeAttribute>(attr => attr.StatusCode == statusCode && (responseType == null || attr.Type == responseType));
        }

        [Fact]
        public async void GetRemarks_Should_Call_IRemarksService_GetRemarks()
        {
            var service = Substitute.For<IRemarksService>();

            var filter = "filter";
            await _controller.GetRemarks(filter, service);

            await service.Received(1).GetRemarks(filter);
        }

        [Fact]
        public async void GetRemarks_Should_Return_Correctly()
        {
            IEnumerable<RemarkDetails> response = new [] { new RemarkDetails() };
            var service = Substitute.For<IRemarksService>();
            service.GetRemarks(Arg.Any<string>()).Returns(response);

            var actual = await _controller.GetRemarks("any", service);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be($"{response.Count()} remark(s) found.");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(response);
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
        public void AddRemark_Should_Include_ProducesAttribute()
        {
            var methodName = nameof(_controller.AddRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<ProducesAttribute>()
                .Which.ContentTypes.Should().Contain("application/json");
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

        [Fact]
        public async void AddRemark_Should_Call_IUserDetailsProvider_GetUserDetails()
        {
            var provider = Substitute.For<IUserDetailsProvider>();
            provider.GetUserDetails().Returns(new UserDetails());

            await _controller.AddRemark(new AddRemarkRequest(), Substitute.For<IRemarksService>(), provider);

            provider.Received(1).GetUserDetails();
        }

        [Fact]
        public async void AddRemark_Should_Call_IRemarksService_AddRemark()
        {
            var request = new AddRemarkRequest();

            var service = Substitute.For<IRemarksService>();

            var email = "email";
            var provider = Substitute.For<IUserDetailsProvider>();
            provider.GetUserDetails().Returns(new UserDetails {Email = email});

            await _controller.AddRemark(request, service, provider);

            await service.Received(1).AddRemark(email, request);
        }

        [Fact]
        public async void AddRemark_Should_Return_Correctly()
        {
            var response = new RemarkDetails();
            var service = Substitute.For<IRemarksService>();
            service.AddRemark(Arg.Any<string>(), Arg.Any<AddRemarkRequest>()).Returns(response);

            var provider = Substitute.For<IUserDetailsProvider>();
            provider.GetUserDetails().Returns(new UserDetails());

            var actual = await _controller.AddRemark(new AddRemarkRequest(), service, provider);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status201Created);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("Remark created.");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(response);
        }

        [Fact]
        public void UpdateRemark_Should_Include_HttpPatchAttribute()
        {
            var methodName = nameof(_controller.UpdateRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<HttpPatchAttribute>()
                .Which.Template.Should().Be("{remarkId}");
        }

        [Fact]
        public void UpdateRemark_Should_Include_AuthorizeAttribute()
        {
            var methodName = nameof(_controller.UpdateRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void UpdateRemark_Should_Include_ProducesAttribute()
        {
            var methodName = nameof(_controller.UpdateRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<ProducesAttribute>()
                .Which.ContentTypes.Should().Contain("application/json");
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK, null)]
        [InlineData(StatusCodes.Status400BadRequest, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status401Unauthorized, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status403Forbidden, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status500InternalServerError, typeof(ApiResponse))]
        public void UpdateRemark_Should_Include_ProducesResponseTypeAttribute(int statusCode, Type responseType)
        {
            var methodName = nameof(_controller.UpdateRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<ProducesResponseTypeAttribute>(attr => attr.StatusCode == statusCode && (responseType == null || attr.Type == responseType));
        }

        [Fact]
        public async void UpdateRemark_Should_Call_IRemarksService_UpdateRemark()
        {
            var remarkId = "remarkId";
            var service = Substitute.For<IRemarksService>();

            var request = new UpdateRemarkRequest();
            await _controller.UpdateRemark(remarkId, request, service);

            await service.Received(1).UpdateRemark(remarkId, request);
        }

        [Fact]
        public async void UpdateRemark_Should_Return_Correctly()
        {
            var remarkId = "remarkId";
            var service = Substitute.For<IRemarksService>();

            var actual = await _controller.UpdateRemark(remarkId, new UpdateRemarkRequest(), service);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("Remark updated.");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(remarkId);
        }

        [Fact]
        public void DeleteRemark_Should_Include_HttpDeleteAttribute()
        {
            var methodName = nameof(_controller.DeleteRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<HttpDeleteAttribute>()
                .Which.Template.Should().Be("{remarkId}");
        }

        [Fact]
        public void DeleteRemark_Should_Include_AuthorizeAttribute()
        {
            var methodName = nameof(_controller.DeleteRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void DeleteRemark_Should_Include_ProducesAttribute()
        {
            var methodName = nameof(_controller.DeleteRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName)
                .Should().BeDecoratedWith<ProducesAttribute>()
                .Which.ContentTypes.Should().Contain("application/json");
        }

        [Theory]
        [InlineData(StatusCodes.Status200OK, null)]
        [InlineData(StatusCodes.Status401Unauthorized, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status403Forbidden, typeof(ApiResponse))]
        [InlineData(StatusCodes.Status500InternalServerError, typeof(ApiResponse))]
        public void DeleteRemark_Should_Include_ProducesResponseTypeAttribute(int statusCode, Type responseType)
        {
            var methodName = nameof(_controller.DeleteRemark);
            var t = _controller.GetType();
            t.GetMethod(methodName).Should().BeDecoratedWith<ProducesResponseTypeAttribute>(attr => attr.StatusCode == statusCode && (responseType == null || attr.Type == responseType));
        }

        [Fact]
        public async void DeleteRemark_Should_Call_IRemarksService_DeleteRemark()
        {
            var remarkId = "remarkId";
            var service = Substitute.For<IRemarksService>();

            await _controller.DeleteRemark(remarkId, service);

            await service.Received(1).DeleteRemark(remarkId);
        }

        [Fact]
        public async void DeleteRemark_Should_Return_Correctly()
        {
            var remarkId = "remarkId";
            var service = Substitute.For<IRemarksService>();

            var actual = await _controller.DeleteRemark(remarkId, service);

            actual.Should().BeOfType<ActionResult<ApiResponse>>();

            actual.Result.Should().BeOfType<ObjectResult>();
            actual.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);

            actual.Result.As<ObjectResult>().Value.Should().BeOfType<ApiResponse>();
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().Be(true);
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("Remark deleted.");
            actual.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().Be(remarkId);
        }
    }
}
