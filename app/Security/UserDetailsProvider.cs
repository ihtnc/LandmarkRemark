using System.Linq;
using Microsoft.AspNetCore.Http;

namespace LandmarkRemark.Api.Security
{
    public interface IUserDetailsProvider
    {
        UserDetails GetUserDetails();
    }

    public class UserDetailsProvider : IUserDetailsProvider
    {
        private readonly IHttpContextAccessor _accessor;

        public UserDetailsProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public UserDetails GetUserDetails()
        {
            var principal = _accessor.HttpContext?.User;
            var userId = principal?.Claims?.SingleOrDefault(c => c.Type == "user_id");

            return userId == null ? null : new UserDetails
            {
                UserId = userId.Value
            };
        }
    }
}