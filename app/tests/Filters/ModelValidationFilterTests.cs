using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using LandmarkRemark.Api.Filters;
using LandmarkRemark.Api.Models;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace LandmarkRemark.Api.Tests.Filters
{
    public class ModelValidationFilterTests
    {
        private readonly IActionFilter _filter;

        public ModelValidationFilterTests()
        {
            _filter = new ModelValidationFilter();
        }

        [Fact]
        public void OnActionExecuting_Should_Update_Result_When_Model_Is_Invalid()
        {
            var actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };
            var context = new ActionExecutingContext(actionContext, Substitute.For<IList<IFilterMetadata>>(), Substitute.For<IDictionary<string, object>>(), null);

            var field = "field";
            var message = "validation message";
            actionContext.ModelState.AddModelError(field, message);
            var errors = new ModelErrorCollection();
            errors.Add(message);
            var data = new [] { new { Key = field, Errors = errors } };

            _filter.OnActionExecuting(context);

            context.Result.Should().BeAssignableTo<ObjectResult>();
            context.Result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            context.Result.As<ObjectResult>().Value.Should().BeAssignableTo<ApiResponse>();
            context.Result.As<ObjectResult>().Value.As<ApiResponse>().Success.Should().BeFalse();
            context.Result.As<ObjectResult>().Value.As<ApiResponse>().Message.Should().Be("Invalid request format.");
            context.Result.As<ObjectResult>().Value.As<ApiResponse>().Data.Should().BeEquivalentTo(data);
        }

        [Fact]
        public void OnActionExecuting_Should_Not_Update_Result_When_Model_Is_Valid()
        {
            var actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };
            var context = new ActionExecutingContext(actionContext, Substitute.For<IList<IFilterMetadata>>(), Substitute.For<IDictionary<string, object>>(), null);

            _filter.OnActionExecuting(context);

            context.Result.Should().BeNull();
        }
    }
}