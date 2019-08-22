using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
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

        private readonly HttpContext _context;

        public UserDetailsProviderTests()
        {
            _accessor = Substitute.For<IHttpContextAccessor>();
            _provider = new UserDetailsProvider(_accessor);

            _context = new DefaultHttpContext();
            _accessor.HttpContext.Returns(_context);
        }

        [Fact]
        public void GetUserDetails_Should_Return_Correctly()
        {
            var userId = "userId";
            var userIdClaim = new Claim("user_id", userId);

            var email = "email";
            var firebase = new JObject
            {
                ["identities"] = new JObject
                {
                    ["email"] = new JArray { email }
                }
            };
            var firebaseClaim = new Claim("firebase", firebase.ToString());

            var identity = new ClaimsIdentity(new [] {userIdClaim, firebaseClaim});
            var principal = new ClaimsPrincipal(new [] {identity});
            _context.User = principal;

            var userDetails = _provider.GetUserDetails();

            userDetails.UserId.Should().Be(userId);
            userDetails.Email.Should().Be(email);
        }

        [Fact]
        public void GetUserDetails_Should_Handle_Missing_UserId_Claim()
        {
            var userId = "userId";
            var userIdClaim = new Claim("not_user_id", userId);

            var email = "email";
            var firebase = new JObject
            {
                ["identities"] = new JObject
                {
                    ["email"] = new JArray { email }
                }
            };
            var firebaseClaim = new Claim("firebase", firebase.ToString());

            var identity = new ClaimsIdentity(new [] {userIdClaim, firebaseClaim});
            var principal = new ClaimsPrincipal(new [] {identity});
            _context.User = principal;

            var userDetails = _provider.GetUserDetails();

            userDetails.Should().BeNull();
        }

        [Fact]
        public void GetUserDetails_Should_Handle_Missing_Firebase_Claim()
        {
            var userId = "userId";
            var userIdClaim = new Claim("user_id", userId);

            var email = "email";
            var firebase = new JObject
            {
                ["identities"] = new JObject
                {
                    ["email"] = new JArray { email }
                }
            };
            var firebaseClaim = new Claim("not_firebase", firebase.ToString());

            var identity = new ClaimsIdentity(new [] {userIdClaim, firebaseClaim});
            var principal = new ClaimsPrincipal(new [] {identity});
            _context.User = principal;

            var userDetails = _provider.GetUserDetails();

            userDetails.Should().BeNull();
        }

        [Fact]
        public void GetUserDetails_Should_Handle_Empty_Claims()
        {
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(new [] {identity});
            _context.User = principal;

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