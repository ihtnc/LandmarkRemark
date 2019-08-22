using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using LandmarkRemark.Api.Http;
using Xunit;
using FluentAssertions;
using FluentAssertions.Json;

namespace LandmarkRemark.Api.Tests.Http
{
    public class ApiRequestProviderTests
    {
        private readonly IApiRequestProvider _provider;

        public ApiRequestProviderTests()
        {
            _provider = new ApiRequestProvider();
        }

        [Fact]
        public void CreateGetRequest_Should_Set_HttpMethod_Correctly()
        {
            _provider.CreateGetRequest("https://google.com").Method.Should().Be(HttpMethod.Get);
        }

        [Fact]
        public void CreateGetRequest_Should_Set_Uri_Correctly()
        {
            var url = "https://google.com";
            _provider.CreateGetRequest(url).RequestUri.OriginalString.Should().Be(url);
        }

        [Fact]
        public void CreatePostRequest_Should_Set_HttpMethod_Correctly()
        {
            _provider.CreatePostRequest("https://google.com", 123).Method.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public void CreatePostRequest_Should_Set_Uri_Correctly()
        {
            var url = "https://google.com";
            _provider.CreatePostRequest(url, 123).RequestUri.OriginalString.Should().Be(url);
        }

        [Fact]
        public void CreateDeleteRequest_Should_Set_HttpMethod_Correctly()
        {
            _provider.CreateDeleteRequest("https://google.com").Method.Should().Be(HttpMethod.Delete);
        }

        [Fact]
        public void CreateDeleteRequest_Should_Set_HttpMethod_Correctly_Should_Set_Uri_Correctly()
        {
            var url = "https://google.com";
            _provider.CreateDeleteRequest(url).RequestUri.OriginalString.Should().Be(url);
        }

        [Fact]
        public void CreateRequest_Should_Set_HttpMethod_Correctly()
        {
            var url = "https://google.com";
            _provider.CreateRequest<object>(HttpMethod.Get, url).Method.Should().Be(HttpMethod.Get);
            _provider.CreateRequest(HttpMethod.Put, url, content: 123).Method.Should().Be(HttpMethod.Put);
            _provider.CreateRequest(HttpMethod.Post, url, content: 123).Method.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public void CreateRequest_Should_Set_Uri_Correctly()
        {
            var url = "https://google.com";
            _provider.CreateRequest<object>(HttpMethod.Get, url).RequestUri.OriginalString.Should().Be(url);
        }

        [Fact]
        public void CreateRequest_Should_Set_Headers_Correctly()
        {
            var headers = new Dictionary<string, string>
            {
                {"header1", "value1"},
                {"header2", "value2"},
            };

            var request = _provider.CreateRequest<object>(HttpMethod.Get, "https://google.com", headers: headers);

            foreach(var item in headers)
            {
                request.Headers.GetValues(item.Key).Should().OnlyContain(s => string.Equals(s, item.Value));
            }
        }

        [Fact]
        public void CreateRequest_Should_Handle_Empty_Headers_Parameter()
        {
            var request = _provider.CreateRequest<object>(HttpMethod.Get, "https://google.com", headers: new Dictionary<string, string>());

            request.Headers.Should().BeEmpty();
        }

        [Fact]
        public void CreateRequest_Should_Handle_Null_Headers_Parameter()
        {
            var request = _provider.CreateRequest<object>(HttpMethod.Get, "https://google.com");

            request.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void CreateRequest_Should_Set_Content_Using_Default_Mapper_If_Not_Supplied()
        {
            var requestObj = new
            {
                StringProp = "string",
                IntProp = 123
            };
            var request = JToken.FromObject(requestObj);

            var actual = _provider.CreateRequest(HttpMethod.Post, "https://google.com", content: request);

            actual.Content.Headers.GetValues("content-type").Should().OnlyContain(s => string.Equals(s, "application/json; charset=utf-8"));

            var content = await actual.Content.ReadAsStringAsync();
            var token = JToken.Parse(content);
            token.Should().BeEquivalentTo(request);
        }

        [Fact]
        public async void CreateRequest_Should_Set_Content_Using_Custom_Mapper_If_Supplied()
        {
            var request = "requestBody";

            var actual = _provider.CreateRequest(HttpMethod.Post, "https://google.com", content: request, contentMapper: (value) => new StringContent(request, Encoding.UTF8, "application/text"));

            actual.Content.Headers.GetValues("content-type").Should().OnlyContain(s => string.Equals(s, "application/text; charset=utf-8"));

            var content = await actual.Content.ReadAsStringAsync();
            content.Should().Be(request);
        }

        [Fact]
        public void CreateRequest_Should_Not_Set_Content_If_Content_Is_Null()
        {
            var actual = _provider.CreateRequest<object>(HttpMethod.Post, "https://google.com", content: null);

            actual.Content.Should().BeNull();
        }

        [Fact]
        public void CreateRequest_Should_Handle_Null_Content_Parameter()
        {
            var actual = _provider.CreateRequest<object>(HttpMethod.Get, "https://google.com");

            actual.Content.Should().BeNull();
        }

        [Fact]
        public void CreateRequest_Should_Set_QueryString_Correctly()
        {
            var queries = new Dictionary<string, string>
            {
                {"query1", "value1"},
                {"query2", "value2"},
            };

            var request = _provider.CreateRequest<object>(HttpMethod.Get, "https://google.com", queries: queries);

            var list = queries.Select(q => $"{q.Key}={q.Value}");
            var queryString = $"?{string.Join('&', list)}";
            request.RequestUri.Query.Should().Be(queryString);
        }

        [Fact]
        public void CreateRequest_Should_Append_QueryString_If_Existing()
        {
            var queries = new Dictionary<string, string>
            {
                {"query1", "value1"},
                {"query2", "value2"},
            };

            var request = _provider.CreateRequest<object>(HttpMethod.Get, "https://google.com?originalQuery=originalValue", queries: queries);

            var list = queries.Select(q => $"{q.Key}={q.Value}");
            var queryString = $"?originalQuery=originalValue&{string.Join('&', list)}";
            request.RequestUri.Query.Should().Be(queryString);
        }

        [Fact]
        public void CreateRequest_Should_Handle_Empty_Queries_Parameter()
        {
            var request = _provider.CreateRequest<object>(HttpMethod.Get, "https://google.com", queries: new Dictionary<string, string>());

            request.RequestUri.Query.Should().BeEmpty();
        }

        [Fact]
        public void CreateRequest_Should_Handle_Null_Queries_Parameter()
        {
            var request = _provider.CreateRequest<object>(HttpMethod.Get, "https://google.com");

            request.RequestUri.Query.Should().BeEmpty();
        }
    }
}