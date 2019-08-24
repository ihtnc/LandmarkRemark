using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace LandmarkRemark.Api.Http
{
    public interface IApiClient
    {
        Task<T> Send<T>(HttpRequestMessage request);
        Task<T> Send<T>(HttpRequestMessage request, Func<HttpResponseMessage, Task<T>> responseMapper);
    }

    public class ApiClient : IApiClient
    {
        private readonly IHttpClientFactory _clientFactory;

        public ApiClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> Send<T>(HttpRequestMessage request)
        {
            return await Send<T>(request, null);
        }

        public async Task<T> Send<T>(HttpRequestMessage request, Func<HttpResponseMessage, Task<T>> responseMapper)
        {
            using (var client = _clientFactory.CreateClient())
            {
                using (var responseMessage = await client.SendAsync(request))
                {
                    responseMessage.EnsureSuccessStatusCode();
                    if (responseMapper == null)
                    {
                        var content = await responseMessage.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(content);
                    }
                    else
                    {
                        return await responseMapper(responseMessage);
                    }
                }
            }
        }
    }
}