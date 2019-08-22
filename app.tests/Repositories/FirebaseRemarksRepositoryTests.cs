using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using LandmarkRemark.Api.Config;
using LandmarkRemark.Api.Http;
using LandmarkRemark.Api.Repositories;
using LandmarkRemark.Api.Repositories.Models;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace LandmarkRemark.Api.Tests.Repositories
{
    public class FirebaseDatabaseRepositoryTests
    {
        private readonly string _firebaseDatabase;
        private readonly string _firebaseApiKey;
        private readonly string _requestAuthorisation;
        private readonly IApiClient _apiClient;
        private readonly IApiRequestProvider _requestProvider;
        private readonly IHttpContextAccessor _accessor;

        private readonly IRemarksRepository _repository;

        public FirebaseDatabaseRepositoryTests()
        {
            _firebaseDatabase = "firebaseDatabaseUrl";
            _firebaseApiKey = "firebaseApiKey";
            _requestAuthorisation = "Bearer token";

            var options = Substitute.For<IOptions<FirebaseConfig>>();
            options.Value.Returns(new FirebaseConfig { Database = _firebaseDatabase, ApiKey = _firebaseApiKey });

            _apiClient = Substitute.For<IApiClient>();
            _requestProvider = Substitute.For<IApiRequestProvider>();

            var context = new DefaultHttpContext();
            context.Request.Headers.Add("Authorization", _requestAuthorisation);
            _accessor = Substitute.For<IHttpContextAccessor>();
            _accessor.HttpContext.Returns(context);

            _repository = new FirebaseRemarksRepository(options, _apiClient, _requestProvider, _accessor);
        }

        [Fact]
        public async void AddRemark_Should_Call_IApiRequestProvider_CreatePostRequest()
        {
            JObject content = null;
            Dictionary<string, string> header = null;
            Dictionary<string, string> query = null;
            _requestProvider.CreatePostRequest(Arg.Any<string>(), Arg.Do<JObject>(a => content = a), headers: Arg.Do<Dictionary<string, string>>(a => header = a), queries: Arg.Do<Dictionary<string, string>>(a => query = a));

            var details = new RemarkDetails
            {
                Latitude = 123,
                Longitude = 456,
                Remark = "remarks",
                UserId = "userId"
            };
            await _repository.AddRemark(details);

            _requestProvider.Received(1).CreatePostRequest(_firebaseDatabase, Arg.Any<JObject>(), headers: Arg.Any<Dictionary<string, string>>(), queries: Arg.Any<Dictionary<string, string>>());

            content.Should().ContainKey("fields");
            content.SelectToken("fields").Value<JObject>().Should().ContainKey("lat");
            content.SelectToken("fields.lat").Value<JObject>().Should().ContainKey("doubleValue");
            content.SelectToken("fields.lat.doubleValue").Value<double>().Should().Be(details.Latitude);
            content.SelectToken("fields").Value<JObject>().Should().ContainKey("lng");
            content.SelectToken("fields.lng").Value<JObject>().Should().ContainKey("doubleValue");
            content.SelectToken("fields.lng.doubleValue").Value<double>().Should().Be(details.Longitude);
            content.SelectToken("fields").Value<JObject>().Should().ContainKey("remark");
            content.SelectToken("fields.remark").Value<JObject>().Should().ContainKey("stringValue");
            content.SelectToken("fields.remark.stringValue").Value<string>().Should().Be(details.Remark);
            content.SelectToken("fields").Value<JObject>().Should().ContainKey("uid");
            content.SelectToken("fields.uid").Value<JObject>().Should().ContainKey("stringValue");
            content.SelectToken("fields.uid.stringValue").Value<string>().Should().Be(details.UserId);

            header.Should().ContainKey("Authorization");
            header["Authorization"].Should().Be(_requestAuthorisation);

            query.Should().ContainKey("key");
            query["key"].Should().Be(_firebaseApiKey);
        }

        [Fact]
        public async void AddRemark_Should_Call_IApiClient_Send()
        {
            Func<HttpResponseMessage, Task<RemarkDetails>> mapper = null;
            var request = new HttpRequestMessage();
            _requestProvider.CreatePostRequest(Arg.Any<string>(), Arg.Any<JObject>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Dictionary<string, string>>()).Returns(request);
            await _apiClient.Send(Arg.Any<HttpRequestMessage>(), Arg.Do<Func<HttpResponseMessage, Task<RemarkDetails>>>(a => mapper = a));

            await _repository.AddRemark(new RemarkDetails());

            await _apiClient.Received(1).Send<RemarkDetails>(request, Arg.Any<Func<HttpResponseMessage, Task<RemarkDetails>>>());

            var expected = new RemarkDetails
            {
                RemarkId = "remarkId",
                Latitude = 123,
                Longitude = 456,
                Remark = "remarks",
                UserId = "userId"
            };
            var body = new JObject
            {
                ["name"] = $"remarks/{expected.RemarkId}",
                ["fields"] = new JObject
                {
                    ["lat"] = new JObject{ ["doubleValue"] = expected.Latitude },
                    ["lng"] = new JObject{ ["doubleValue"] = expected.Longitude },
                    ["remark"] = new JObject{ ["stringValue"] = expected.Remark },
                    ["uid"] = new JObject{ ["stringValue"] = expected.UserId }
                }
            };

            var response = new HttpResponseMessage();
            response.Content = new StringContent(body.ToString());
            var actual = await mapper(response);
            actual.RemarkId.Should().Be(expected.RemarkId);
            actual.Latitude.Should().Be(expected.Latitude);
            actual.Longitude.Should().Be(expected.Longitude);
            actual.Remark.Should().Be(expected.Remark);
            actual.UserId.Should().Be(expected.UserId);

            request.Dispose();
        }

        [Fact]
        public async void AddRemark_Should_Call_Return_Correctly()
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreatePostRequest(Arg.Any<string>(), Arg.Any<JObject>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Dictionary<string, string>>()).Returns(request);

            var response = new RemarkDetails();
            _apiClient.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<Func<HttpResponseMessage, Task<RemarkDetails>>>()).Returns(response);

            var actual = await _repository.AddRemark(new RemarkDetails());
            actual.Should().Be(response);

            request.Dispose();
        }

        [Fact]
        public async void DeleteRemark_Should_Call_IApiRequestProvider_CreateDeleteRequest()
        {
            Dictionary<string, string> header = null;
            Dictionary<string, string> query = null;
            _requestProvider.CreateDeleteRequest(Arg.Any<string>(), headers: Arg.Do<Dictionary<string, string>>(a => header = a), queries: Arg.Do<Dictionary<string, string>>(a => query = a));

            var remarkId = "remarkId";
            await _repository.DeleteRemark(remarkId);

            _requestProvider.Received(1).CreateDeleteRequest($"{_firebaseDatabase}/{remarkId}", headers: Arg.Any<Dictionary<string, string>>(), queries: Arg.Any<Dictionary<string, string>>());

            header.Should().ContainKey("Authorization");
            header["Authorization"].Should().Be(_requestAuthorisation);

            query.Should().ContainKey("key");
            query["key"].Should().Be(_firebaseApiKey);
        }

        [Fact]
        public async void DeleteRemark_Should_Call_IApiClient_Send()
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreateDeleteRequest(Arg.Any<string>(), Arg.Any<Dictionary<string, string>>(), Arg.Any<Dictionary<string, string>>()).Returns(request);

            await _repository.DeleteRemark("remarkId");

            await _apiClient.Received(1).Send<object>(request);

            request.Dispose();
        }
    }
}