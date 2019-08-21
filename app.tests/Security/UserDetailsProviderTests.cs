using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using LandmarkRemark.Api.Security;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace LandmarkRemark.Api.Tests.Security
{
    public class UserDetailsProviderTests
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserDetailsProvider _provider;

        public UserDetailsProviderTests()
        {
            _accessor = Substitute.For<IHttpContextAccessor>();
            _provider = new UserDetailsProvider(_accessor);
        }

        [Fact]
        public void GetUserDetails_Should_Return_Correctly()
        {
            var userId = "userId";
            var userIdClaim = new Claim("user_id", userId);
            var identity = new ClaimsIdentity(new [] {userIdClaim});
            var principal = new ClaimsPrincipal(new [] {identity});
            var context = new DefaultHttpContext
            {
                User = principal
            };
            _accessor.HttpContext.Returns(context);

            var userDetails = _provider.GetUserDetails();

            userDetails.UserId.Should().Be(userId);
        }

        [Fact]
        public void GetUserDetails_Should_Handle_Missing_Claim()
        {
            var userId = "userId";
            var userIdClaim = new Claim("not_user_id", userId);
            var identity = new ClaimsIdentity(new [] {userIdClaim});
            var principal = new ClaimsPrincipal(new [] {identity});
            var context = new DefaultHttpContext
            {
                User = principal
            };
            _accessor.HttpContext.Returns(context);

            var userDetails = _provider.GetUserDetails();

            userDetails.Should().BeNull();
        }

        [Fact]
        public void GetUserDetails_Should_Handle_Empty_Claims()
        {
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(new [] {identity});
            var context = new DefaultHttpContext
            {
                User = principal
            };
            _accessor.HttpContext.Returns(context);

            var userDetails = _provider.GetUserDetails();

            userDetails.Should().BeNull();
        }

        [Fact]
        public void GetUserDetails_Should_Handle_Null_HttpContext()
        {
            _accessor.HttpContext.Returns((HttpContext)null);

            var userDetails = _provider.GetUserDetails();

            userDetails.Should().BeNull();
        }
    }
}