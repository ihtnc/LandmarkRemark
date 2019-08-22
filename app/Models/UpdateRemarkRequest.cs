using Newtonsoft.Json;

namespace LandmarkRemark.Api.Models
{
    public class UpdateRemarkRequest
    {
        [JsonRequired]
        public string Remark { get; set; }
    }
}