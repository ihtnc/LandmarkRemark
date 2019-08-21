using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using LandmarkRemark.Api.Filters;
using LandmarkRemark.Api.Models;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace LandmarkRemark.Api.Tests.Filters
{
    public class AuthoriseFilterTests
    {
        private readonly string _authenticationScheme;
        private readonly AuthorizationPolicy _policy;
        private readonly IAsyncAuthorizationFilter _filter;

        private readonly ActionContext _actionContext;

        public AuthoriseFilterTests()
        {
            _authenticationScheme = "scheme";
            _policy = new AuthorizationPolicy(new [] { Substitute.For<IAuthorizationRequirement>() }, new [] { _authenticationScheme });
            _filter = new AuthoriseFilter(_policy);

            _actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };
        }

        [Fact]
        public void OnAuthorizationAsync_Should_Throw_An_Exception_When_Context_Is_Null()
        {
            Func<Task> call = async () => await _filter.OnAuthorizationAsync(null);
            call.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("context");
        }

        [Fact]
        public void OnAuthorizationAsync_Should_Skip_Filter_When_Context_Has_IAllowAnonymoustFilter()
        {
            var context = new AuthorizationFilterContext(_actionContext, new [] { Substitute.For<IAllowAnonymousFilter>() } );
            _filter.OnAuthorizationAsync(context);

            context.Result.Should().BeNull();
        }

        [Fact]
        public void OnAuthorizationAsync_Should_Call_GetRequiredService()
        {
            _actionContext.HttpContext.RequestServices = Substitute.For<IServiceProvider>();

            var context = new AuthorizationFilterContext(_actionContext, Substitute.For<IList<IFilterMetadata>>());
            _filter.OnAuthorizationAsync(context);

            _actionContext.HttpContext.RequestServices.Received(1).GetService(typeof(IPolicyEvaluator));
        }

        [Fact]
        public void OnAuthorizationAsync_Should_Call_AuthenticateAsync()
        {
            var policyEvaluator = Substitute.For<IPolicyEvaluator>();
            _actionContext.HttpContext.RequestServices = Substitute.For<IServiceProvider>();
            _actionContext.HttpContext.RequestServices.GetService(Arg.Any<Type>()).Returns(policyEvaluator);

            var context = new AuthorizationFilterContext(_actionContext, Substitute.For<IList<IFilterMetadata>>());
            _filter.OnAuthorizationAsync(context);

            policyEvaluator.Received(1).AuthenticateAsync(_policy, _actionContext.HttpContext);
        }

        [Fact]
        public void OnAuthorizationAsync_Should_Call_AuthorizeAsync()
        {
            var policyEvaluator = Substitute.For<IPolicyEvaluator>();
            _actionContext.HttpContext.RequestServices = Substitute.For<IServiceProvider>();
            _actionContext.HttpContext.RequestServices.GetService(Arg.Any<Type>()).Returns(policyEvaluator);

            var authenticationResult = AuthenticateResult.NoResult();
            policyEvaluator.AuthenticateAsync(Arg.Any<AuthorizationPolicy>(), Arg.Any<HttpContext>()).Returns(authenticationResult);

            var context = new AuthorizationFilterContext(_actionContext, Substitute.For<IList<IFilterMetadata>>());
            _filter.OnAuthorizationAsync(context);

            policyEvaluator.Received(1).AuthorizeAsync(_policy, authenticationResult, context.HttpContext, context);
        }

        [Fact]
        public void OnAuthorizationAsync_Should_Set_Result_When_Authorization_Is_Challenged()
        {
            var policyEvaluator = Substitute.For<IPolicyEvaluator>();
            _actionContext.HttpContext.RequestServices = Substitute.For<IServiceProvider>();
            _actionContext.HttpContext.RequestServices.GetService(Arg.Any<Type>()).Returns(policyEvaluator);

            var authorizeResult = PolicyAuthorizationResult.Challenge();
            policyEvaluator.AuthorizeAsync(Arg.Any<AuthorizationPolicy>(), Arg.Any<AuthenticateResult>(), Arg.Any<HttpContext>(), Arg.Any<object>()).Returns(authorizeResult);

            var context = new AuthorizationFilterContext(_actionContext, Substitute.For<IList<IFilterMetadata>>());
            _filter.OnAuthorizationAsync(context);

            context.Result.Should().BeAssignableTo<ObjectResult>();
            context.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            context.Result.As<ObjectResult>().Value.Should().BeAssignableTo<ApiResponse>();
            context.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("Unauthorised.");
            context.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeNull();
        }

        [Fact]
        public void OnAuthorizationAsync_Should_Set_Result_When_Authorization_Is_Forbidden()
        {
            var policyEvaluator = Substitute.For<IPolicyEvaluator>();
            _actionContext.HttpContext.RequestServices = Substitute.For<IServiceProvider>();
            _actionContext.HttpContext.RequestServices.GetService(Arg.Any<Type>()).Returns(policyEvaluator);

            var authorizeResult = PolicyAuthorizationResult.Forbid();
            policyEvaluator.AuthorizeAsync(Arg.Any<AuthorizationPolicy>(), Arg.Any<AuthenticateResult>(), Arg.Any<HttpContext>(), Arg.Any<object>()).Returns(authorizeResult);

            var context = new AuthorizationFilterContext(_actionContext, Substitute.For<IList<IFilterMetadata>>());
            _filter.OnAuthorizationAsync(context);

            context.Result.Should().BeAssignableTo<ObjectResult>();
            context.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            context.Result.As<ObjectResult>().Value.Should().BeAssignableTo<ApiResponse>();
            context.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("Forbidden.");
            context.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.As<IEnumerable<string>>().Should().BeEquivalentTo(_authenticationScheme);
        }

        [Fact]
        public void OnAuthorizationAsync_Should_Not_Set_Result_When_Authorization_Succeeds()
        {
            var policyEvaluator = Substitute.For<IPolicyEvaluator>();
            _actionContext.HttpContext.RequestServices = Substitute.For<IServiceProvider>();
            _actionContext.HttpContext.RequestServices.GetService(Arg.Any<Type>()).Returns(policyEvaluator);

            var authorizeResult = PolicyAuthorizationResult.Success();
            policyEvaluator.AuthorizeAsync(Arg.Any<AuthorizationPolicy>(), Arg.Any<AuthenticateResult>(), Arg.Any<HttpContext>(), Arg.Any<object>()).Returns(authorizeResult);

            var context = new AuthorizationFilterContext(_actionContext, Substitute.For<IList<IFilterMetadata>>());
            _filter.OnAuthorizationAsync(context);

            context.Result.Should().BeNull();
        }
    }
}