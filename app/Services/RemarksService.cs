using System.Threading.Tasks;
using LandmarkRemark.Api.Models;
using LandmarkRemark.Api.Repositories;
using LandmarkRemark.Api.Repositories.Models;

namespace LandmarkRemark.Api.Services
{
    public interface IRemarksService
    {
        Task<RemarkDetails> AddRemark(string userId, AddRemarkRequest request);
        Task UpdateRemark(string remarkId, UpdateRemarkRequest request);
        Task DeleteRemark(string remarkId);
    }

    public class RemarksService : IRemarksService
    {
        private readonly IRemarksRepository _repository;

        public RemarksService(IRemarksRepository repository)
        {
            _repository = repository;
        }

        public async Task<RemarkDetails> AddRemark(string userId, AddRemarkRequest request)
        {
            return await _repository.AddRemark(new RemarkDetails
            {
                UserId = userId,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Remark = request.Remark
            });
        }

        public async Task UpdateRemark(string remarkId, UpdateRemarkRequest request)
        {
            await _repository.UpdateRemark(remarkId, new UpdatableRemarkDetails
            {
                Remark = request.Remark
            });
        }

        public async Task DeleteRemark(string remarkId)
        {
            await _repository.DeleteRemark(remarkId);
        }
    }
}