using LandmarkRemark.Api.Models;
using LandmarkRemark.Api.Security;
using LandmarkRemark.Api.Services;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace LandmarkRemark.Api.Tests.Services
{
    public class SecurityServiceTests
    {
        private readonly IFirebaseAuthenticationProvider _provider;
        private readonly ISecurityService _service;

        public SecurityServiceTests()
        {
            _provider = Substitute.For<IFirebaseAuthenticationProvider>();
            _service = new SecurityService(_provider);
        }

        [Fact]
        public async void Register_Should_Call_IFirebaseAuthenticationProvider_SignUp()
        {
            var request = new SecurityRequest();
            await _service.Register(request);

            await _provider.Received(1).SignUp(request);
        }

        [Fact]
        public async void Register_Should_Return_Correctly()
        {
            var expected = new SecurityResponse();
            _provider.SignUp(Arg.Any<SecurityRequest>()).Returns(expected);

            var actual = await _service.Register(new SecurityRequest());

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void LogIn_Should_Call_IFirebaseAuthenticationProvider_SignIn()
        {
            var request = new SecurityRequest();
            await _service.LogIn(request);

            await _provider.Received(1).SignIn(request);
        }

        [Fact]
        public async void LogIn_Should_Return_Correctly()
        {
            var expected = new SecurityResponse();
            _provider.SignIn(Arg.Any<SecurityRequest>()).Returns(expected);

            var actual = await _service.LogIn(new SecurityRequest());

            actual.Should().BeEquivalentTo(expected);
        }
    }
}