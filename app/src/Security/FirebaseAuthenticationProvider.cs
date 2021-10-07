using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using LandmarkRemark.Api.Config;
using LandmarkRemark.Api.Http;
using LandmarkRemark.Api.Models;

namespace LandmarkRemark.Api.Security
{
    public interface IFirebaseAuthenticationProvider
    {
        Task<SecurityResponse> SignUp(SecurityRequest request);
        Task<SecurityResponse> SignIn(SecurityRequest request);
    }

    public class FirebaseAuthenticationProvider : IFirebaseAuthenticationProvider
    {
        private readonly FirebaseConfig _config;
        private readonly IApiClient _apiClient;
        private readonly IApiRequestProvider _requestProvider;

        private readonly Dictionary<string, string> _defaultQuery;

        private readonly string _endpoint;

        public FirebaseAuthenticationProvider(IOptions<FirebaseConfig> options, IApiClient apiClient, IApiRequestProvider requestProvider)
        {
            _config = options.Value ?? throw new InvalidOperationException("Missing config.");
            _apiClient = apiClient;
            _requestProvider = requestProvider;
            _endpoint = $"{_config.AuthEndpoint.TrimEnd('/')}/accounts";

            _defaultQuery = new Dictionary<string, string>
            {
                {"key", _config.ApiKey}
            };
        }

        public async Task<SecurityResponse> SignUp(SecurityRequest details)
        {
            var url = $"{_endpoint}:signUp";
            var content = CreateAuthenticationRequest(details);

            var request = _requestProvider.CreatePostRequest(url, content, queries: _defaultQuery);
            var response = await _apiClient.Send<SecurityResponse>(request);

            return response;
        }

        public async Task<SecurityResponse> SignIn(SecurityRequest details)
        {
            var url = $"{_endpoint}:signInWithPassword";
            var content = CreateAuthenticationRequest(details);

            var request = _requestProvider.CreatePostRequest(url, content, queries: _defaultQuery);
            var response = await _apiClient.Send<SecurityResponse>(request);

            return response;
        }

        private JObject CreateAuthenticationRequest(SecurityRequest request)
        {
            var serializer = new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var obj = JObject.FromObject(request, serializer);
            obj["returnSecureToken"] = true;
            return obj;
        }
    }
}