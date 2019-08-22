using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LandmarkRemark.Api.Http
{
    public interface IApiRequestProvider
    {
        HttpRequestMessage CreateGetRequest(string url, Dictionary<string, string> headers = null, Dictionary<string, string> queries = null);
        HttpRequestMessage CreatePostRequest<T>(string url, T content, Dictionary<string, string> headers = null, Dictionary<string, string> queries = null, Func<T, HttpContent> contentMapper = null);
        HttpRequestMessage CreateDeleteRequest(string url, Dictionary<string, string> headers = null, Dictionary<string, string> queries = null);
        HttpRequestMessage CreateRequest<T>(HttpMethod method, string url, Dictionary<string, string> headers = null, T content = default, Dictionary<string, string> queries = null, Func<T, HttpContent> contentMapper = null);
    }

    public class ApiRequestProvider : IApiRequestProvider
    {
        public HttpRequestMessage CreateGetRequest(string url, Dictionary<string, string> headers = null, Dictionary<string, string> queries = null)
        {
            return CreateRequest<object>(HttpMethod.Get, url, headers: headers, queries: queries);
        }

        public HttpRequestMessage CreatePostRequest<T>(string url, T content, Dictionary<string, string> headers = null, Dictionary<string, string> queries = null, Func<T, HttpContent> contentMapper = null)
        {
            return CreateRequest<T>(HttpMethod.Post, url, headers: headers, content, queries: queries, contentMapper: contentMapper);
        }

        public HttpRequestMessage CreateDeleteRequest(string url, Dictionary<string, string> headers = null, Dictionary<string, string> queries = null)
        {
            return CreateRequest<object>(HttpMethod.Delete, url, headers: headers, queries: queries);
        }

        public HttpRequestMessage CreateRequest<T>(HttpMethod method, string url, Dictionary<string, string> headers = null, T content = default, Dictionary<string, string> queries = null, Func<T, HttpContent> contentMapper = null)
        {
            var requestUrl = url;

            if (queries?.Count > 0)
            {
                var checkUri = new Uri(url);
                var urlHasQueryString = checkUri.Query?.StartsWith('?') == true;
                var list = queries.Select(q => $"{q.Key}={q.Value}");
                var concatenated = string.Join("&", list);
                var queryString = (urlHasQueryString ? "&" : "?") + concatenated;
                requestUrl = url + queryString;
            }

            var message = new HttpRequestMessage(method, requestUrl);

            if (headers?.Count > 0)
            {
                foreach(var item in headers)
                {
                    message.Headers.Add(item.Key, item.Value);
                }
            }

            if (content != null)
            {
                if (contentMapper == null)
                {
                    var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                    var body = JsonConvert.SerializeObject(content, settings);
                    message.Content = new StringContent(body, Encoding.UTF8, "application/json");
                }
                else
                {
                    message.Content = contentMapper(content);
                }
            }

            return message;
        }
    }
}