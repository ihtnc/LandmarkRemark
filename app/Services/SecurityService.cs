using System.Threading.Tasks;
using LandmarkRemark.Api.Models;
using LandmarkRemark.Api.Security;

namespace LandmarkRemark.Api.Services
{
    public interface ISecurityService
    {
        Task<SecurityResponse> Register(SecurityRequest request);
        Task<SecurityResponse> LogIn(SecurityRequest request);
    }

    public class SecurityService : ISecurityService
    {
        private readonly IFirebaseAuthenticationProvider _provider;

        public SecurityService(IFirebaseAuthenticationProvider provider)
        {
            _provider = provider;
        }

        public async Task<SecurityResponse> Register(SecurityRequest request)
        {
            return await _provider.SignUp(request);
        }

        public async Task<SecurityResponse> LogIn(SecurityRequest request)
        {
            return await _provider.SignIn(request);
        }
    }
}