using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using LandmarkRemark.Api.Config;
using LandmarkRemark.Api.Http;
using LandmarkRemark.Api.Repositories.Models;

namespace LandmarkRemark.Api.Repositories
{
    public class FirebaseRemarksRepository : IRemarksRepository
    {
        private readonly FirebaseConfig _config;
        private readonly IApiClient _apiClient;
        private readonly IApiRequestProvider _requestProvider;

        private readonly Dictionary<string, string> _defaultHeader;
        private readonly Dictionary<string, string> _defaultQuery;

        public FirebaseRemarksRepository(IOptions<FirebaseConfig> options, IApiClient apiClient, IApiRequestProvider requestProvider, IHttpContextAccessor contextAccessor)
        {
            _config = options.Value ?? throw new InvalidOperationException("Missing config.");
            _apiClient = apiClient;
            _requestProvider = requestProvider;

            // TODO: REFACTOR
            //   This repository reuses the authorisation header from the original request which is a bad design.
            //   This service should manage its own authorisation with the external API being called.
            contextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorisation);
            var auth = authorisation.ToString();

            // HACK:
            // Firebase is finicky with the OAuth header so we need to fix the header to the casing it accepts.
            if(auth.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
            {
                auth = $"Bearer{auth.Substring(6)}";
            }

            _defaultHeader = new Dictionary<string, string>
            {
                {"Authorization", auth}
            };

            _defaultQuery = new Dictionary<string, string>
            {
                {"key", _config.ApiKey}
            };
        }

        public async Task<RemarkDetails> AddRemark(RemarkDetails remark)
        {
            var content = CreateAddRemarkRequest(remark);

            var request = _requestProvider.CreatePostRequest(_config.Database, content, headers: _defaultHeader, queries: _defaultQuery);
            var response = await _apiClient.Send(request, async resp =>
            {
                var body = await resp.Content.ReadAsStringAsync();
                return GetRemarkDetailsResponse(body);
            });

            return response;
        }

        public async Task DeleteRemark(string remarkId)
        {
            var url = $"{_config.Database.TrimEnd('/')}/{remarkId}";

            var request = _requestProvider.CreateDeleteRequest(url, headers: _defaultHeader, queries: _defaultQuery);
            await _apiClient.Send<object>(request);
        }

        private JObject CreateAddRemarkRequest(RemarkDetails remark)
        {
            return new JObject
            {
                ["fields"] = new JObject
                {
                    ["lat"] = new JObject{ ["doubleValue"] = remark.Latitude },
                    ["lng"] = new JObject{ ["doubleValue"] = remark.Longitude },
                    ["remark"] = new JObject{ ["stringValue"] = remark.Remark },
                    ["uid"] = new JObject{ ["stringValue"] = remark.UserId }
                }
            };
        }

        private RemarkDetails GetRemarkDetailsResponse(string content)
        {
            var obj = JToken.Parse(content);
            var name = obj.Value<string>("name");
            var id = name.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();

            return new RemarkDetails
            {
                RemarkId = id,
                Latitude = obj.SelectToken("fields.lat.doubleValue").Value<double>(),
                Longitude = obj.SelectToken("fields.lng.doubleValue").Value<double>(),
                Remark = obj.SelectToken("fields.remark.stringValue").Value<string>(),
                UserId = obj.SelectToken("fields.uid.stringValue").Value<string>()
            };
        }
    }
}