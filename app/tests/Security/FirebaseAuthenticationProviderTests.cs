using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using LandmarkRemark.Api.Config;
using LandmarkRemark.Api.Http;
using LandmarkRemark.Api.Models;
using LandmarkRemark.Api.Security;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace LandmarkRemark.Api.Tests.Security
{
    public class FirebaseAuthenticationProviderTests
    {
        private readonly string _firebaseAuthEndpoint;
        private readonly string _firebaseApiKey;
        private readonly IApiClient _apiClient;
        private readonly IApiRequestProvider _requestProvider;

        private readonly IFirebaseAuthenticationProvider _provider;

        public FirebaseAuthenticationProviderTests()
        {
            _firebaseAuthEndpoint = "firebaseAuthEndpoint";
            _firebaseApiKey = "firebaseApiKey";

            var options = Substitute.For<IOptions<FirebaseConfig>>();
            options.Value.Returns(new FirebaseConfig { AuthEndpoint = _firebaseAuthEndpoint, ApiKey = _firebaseApiKey });

            _apiClient = Substitute.For<IApiClient>();
            _requestProvider = Substitute.For<IApiRequestProvider>();

            _provider = new FirebaseAuthenticationProvider(options, _apiClient, _requestProvider);
        }

        [Fact]
        public async void SignUp_Should_Call_IApiRequestProvider_CreatePostRequest()
        {
            JObject content = null;
            Dictionary<string, string> query = null;
            _requestProvider.CreatePostRequest(Arg.Any<string>(), Arg.Do<JObject>(a => content = a), Arg.Any<Dictionary<string, string>>(), Arg.Do<Dictionary<string, string>>(a => query = a), Arg.Any<Func<JObject, HttpContent>>());

            var request = new SecurityRequest
            {
                Email = "email",
                Password = "password"
            };
            await _provider.SignUp(request);

            _requestProvider.Received(1).CreatePostRequest($"{_firebaseAuthEndpoint}/accounts:signUp", Arg.Any<JObject>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Dictionary<string, string>>(), null);

            content.Value<string>("email").Should().Be(request.Email);
            content.Value<string>("password").Should().Be(request.Password);
            content.Value<bool>("returnSecureToken").Should().BeTrue();

            query.Should().ContainKey("key");
            query["key"].Should().Be(_firebaseApiKey);
        }

        [Fact]
        public async void SignUp_Should_Call_IApiClient_Send()
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreatePostRequest(Arg.Any<string>(), Arg.Any<JObject>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Func<JObject, HttpContent>>()).Returns(request);
            await _apiClient.Send<SecurityResponse>(Arg.Any<HttpRequestMessage>());

            await _provider.SignUp(new SecurityRequest { Email = "email", Password = "password" });

            await _apiClient.Received(1).Send<SecurityResponse>(request);

            request.Dispose();
        }

        [Fact]
        public async void SignUp_Should_Return_Correctly()
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreatePostRequest(Arg.Any<string>(), Arg.Any<JObject>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Func<JObject, HttpContent>>()).Returns(request);

            var response = new SecurityResponse();
            _apiClient.Send<SecurityResponse>(Arg.Any<HttpRequestMessage>()).Returns(response);

            var actual = await _provider.SignUp(new SecurityRequest { Email = "email", Password = "password" });
            actual.Should().BeSameAs(response);

            request.Dispose();
        }

        [Fact]
        public async void SignIn_Should_Call_IApiRequestProvider_CreatePostRequest()
        {
            JObject content = null;
            Dictionary<string, string> query = null;
            _requestProvider.CreatePostRequest(Arg.Any<string>(), Arg.Do<JObject>(a => content = a), Arg.Any<Dictionary<string, string>>(), Arg.Do<Dictionary<string, string>>(a => query = a), Arg.Any<Func<JObject, HttpContent>>());

            var request = new SecurityRequest
            {
                Email = "email",
                Password = "password"
            };
            await _provider.SignIn(request);

            _requestProvider.Received(1).CreatePostRequest($"{_firebaseAuthEndpoint}/accounts:signInWithPassword", Arg.Any<JObject>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Dictionary<string, string>>(), null);

            content.Value<string>("email").Should().Be(request.Email);
            content.Value<string>("password").Should().Be(request.Password);
            content.Value<bool>("returnSecureToken").Should().BeTrue();

            query.Should().ContainKey("key");
            query["key"].Should().Be(_firebaseApiKey);
        }

        [Fact]
        public async void SignIn_Should_Call_IApiClient_Send()
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreatePostRequest(Arg.Any<string>(), Arg.Any<JObject>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Func<JObject, HttpContent>>()).Returns(request);
            await _apiClient.Send<SecurityResponse>(Arg.Any<HttpRequestMessage>());

            await _provider.SignIn(new SecurityRequest { Email = "email", Password = "password" });

            await _apiClient.Received(1).Send<SecurityResponse>(request);

            request.Dispose();
        }

        [Fact]
        public async void SignIn_Should_Return_Correctly()
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreatePostRequest(Arg.Any<string>(), Arg.Any<JObject>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Func<JObject, HttpContent>>()).Returns(request);

            var response = new SecurityResponse();
            _apiClient.Send<SecurityResponse>(Arg.Any<HttpRequestMessage>()).Returns(response);

            var actual = await _provider.SignIn(new SecurityRequest { Email = "email", Password = "password" });
            actual.Should().BeSameAs(response);

            request.Dispose();
        }
    }
}