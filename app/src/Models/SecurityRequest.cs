using Newtonsoft.Json;

namespace LandmarkRemark.Api.Models
{
    public class SecurityRequest
    {
        [JsonRequired]
        public string Email { get; set; }
        [JsonRequired]
        public string Password { get; set; }
    }
}