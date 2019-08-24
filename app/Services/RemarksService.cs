using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandmarkRemark.Api.Models;
using LandmarkRemark.Api.Repositories;
using LandmarkRemark.Api.Repositories.Models;

namespace LandmarkRemark.Api.Services
{
    public interface IRemarksService
    {
        Task<IEnumerable<RemarkDetails>> GetRemarks(string filter = null);
        Task<RemarkDetails> AddRemark(string email, AddRemarkRequest request);
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

        public async Task<IEnumerable<RemarkDetails>> GetRemarks(string filter = null)
        {
            var list = await _repository.GetRemarks();
            return string.IsNullOrWhiteSpace(filter)
                ? list
                : list.Where(item =>
                    item.Email.Contains(filter, StringComparison.OrdinalIgnoreCase)
                        || item.Remark.Contains(filter, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<RemarkDetails> AddRemark(string email, AddRemarkRequest request)
        {
            return await _repository.AddRemark(new RemarkDetails
            {
                Email = email,
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