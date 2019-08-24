using Newtonsoft.Json;

namespace LandmarkRemark.Api.Models
{
    public class AddRemarkRequest
    {
        [JsonRequired]
        public double Latitude { get; set; }
        [JsonRequired]
        public double Longitude { get; set; }
        [JsonRequired]
        public string Remark { get; set; }
    }
}