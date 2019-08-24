using System.Collections.Generic;
using System.Threading.Tasks;
using LandmarkRemark.Api.Repositories.Models;

namespace LandmarkRemark.Api.Repositories
{
    public interface IRemarksRepository
    {
        Task<IEnumerable<RemarkDetails>> GetRemarks();
        Task<RemarkDetails> AddRemark(RemarkDetails remark);
        Task UpdateRemark(string remarkId, UpdatableRemarkDetails updates);
        Task DeleteRemark(string remarkId);
    }
}