using System.Threading.Tasks;
using LandmarkRemark.Api.Repositories.Models;

namespace LandmarkRemark.Api.Repositories
{
    public interface IRemarksRepository
    {
        Task<RemarkDetails> AddRemark(RemarkDetails remark);
        Task DeleteRemark(string remarkId);
    }
}