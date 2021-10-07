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

        private readonly string _endpoint;

        public FirebaseRemarksRepository(IOptions<FirebaseConfig> options, IApiClient apiClient, IApiRequestProvider requestProvider, IHttpContextAccessor contextAccessor)
        {
            _config = options.Value ?? throw new InvalidOperationException("Missing config.");
            _apiClient = apiClient;
            _requestProvider = requestProvider;

            _endpoint = $"{_config.DbEndPoint.TrimEnd('/')}/projects/{_config.ProjectId}/databases/(default)/documents";

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

        public async Task<IEnumerable<RemarkDetails>> GetRemarks()
        {
            var url = $"{_endpoint}:runQuery";
            var content = CreateGetRemarksRequest();

            var request = _requestProvider.CreatePostRequest(url, content, headers: _defaultHeader);
            var response = await _apiClient.Send(request, async resp =>
            {
                var body = await resp.Content.ReadAsStringAsync();
                var obj = JArray.Parse(body);
                return GetRemarkDetailsArrayResponse(obj);
            });

            return response;
        }

        public async Task<RemarkDetails> AddRemark(RemarkDetails remark)
        {
            var url = $"{_endpoint}/remarks";
            var content = CreateAddRemarkRequest(remark);

            var request = _requestProvider.CreatePostRequest(url, content, headers: _defaultHeader, queries: _defaultQuery);
            var response = await _apiClient.Send(request, async resp =>
            {
                var body = await resp.Content.ReadAsStringAsync();
                var obj = JToken.Parse(body);
                return GetRemarkDetailsResponse(obj);
            });

            return response;
        }

        public async Task UpdateRemark(string remarkId, UpdatableRemarkDetails updates)
        {
            var url = $"{_endpoint}/remarks/{remarkId}";
            var content = CreateUpdateRemarkRequest(updates);

            var updateQuery = new Dictionary<string, string>(_defaultQuery);
            updateQuery.Add("currentDocument.exists", "true");
            updateQuery.Add("updateMask.fieldPaths", "remark");

            var request = _requestProvider.CreatePatchRequest(url, content, headers: _defaultHeader, queries: updateQuery);
            await _apiClient.Send<object>(request);
        }

        public async Task DeleteRemark(string remarkId)
        {
            var url = $"{_endpoint}/remarks/{remarkId}";

            var request = _requestProvider.CreateDeleteRequest(url, headers: _defaultHeader, queries: _defaultQuery);
            await _apiClient.Send<object>(request);
        }

        private JObject CreateGetRemarksRequest()
        {
            var select = new JObject
            {
                ["fields"] = new JArray
                {
                    new JObject { ["fieldPath"] = "lat" },
                    new JObject { ["fieldPath"] = "lng" },
                    new JObject { ["fieldPath"] = "remark" },
                    new JObject { ["fieldPath"] = "email" }
                }
            };

            var from = new JArray
            {
                new JObject { ["collectionId"] = "remarks" }
            };

            var orderBy = new JArray
            {
                new JObject
                {
                    ["field"] = new JObject { ["fieldPath"] = "email" }
                }
            };

            var query = new JObject
            {
                ["structuredQuery"] = new JObject
                {
                    ["select"] = select,
                    ["from"] = from,
                    ["orderBy"] = orderBy
                }
            };

            return query;
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
                    ["email"] = new JObject{ ["stringValue"] = remark.Email }
                }
            };
        }

        private JObject CreateUpdateRemarkRequest(UpdatableRemarkDetails updates)
        {
            return new JObject
            {
                ["fields"] = new JObject
                {
                    ["remark"] = new JObject{ ["stringValue"] = updates.Remark }
                }
            };
        }

        private IEnumerable<RemarkDetails> GetRemarkDetailsArrayResponse(JArray content)
        {
            var list = new List<RemarkDetails>();
            foreach(var item in content)
            {
                var document = item.SelectToken("document");
                if(document == null) { continue; }

                var obj = GetRemarkDetailsResponse(document);
                list.Add(obj);
            }

            return list;
        }
        private RemarkDetails GetRemarkDetailsResponse(JToken content)
        {
            var name = content.Value<string>("name");
            var id = name.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();

            return new RemarkDetails
            {
                RemarkId = id,
                Latitude = content.SelectToken("fields.lat.doubleValue").Value<double>(),
                Longitude = content.SelectToken("fields.lng.doubleValue").Value<double>(),
                Remark = content.SelectToken("fields.remark.stringValue").Value<string>(),
                Email = content.SelectToken("fields.email.stringValue").Value<string>()
            };
        }
    }
}