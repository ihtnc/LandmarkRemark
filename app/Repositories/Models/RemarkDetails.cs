namespace LandmarkRemark.Api.Repositories.Models
{
    public class RemarkDetails
    {
        public string RemarkId { get; set; }
        public string UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Remark { get; set; }
    }
}