using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

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
            if(userId == null) { return null; }

            var firebase = principal?.Claims?.SingleOrDefault(c => c.Type == "firebase");
            if(firebase == null) { return null; }

            var firebaseClaim = firebase != null ? JToken.Parse(firebase.Value) : null;
            var emails = firebaseClaim?.SelectToken("identities.email").Value<JArray>();

            // ASSUMPTION: Firebase claims has an array of emails, but it should only have one
            var email = emails.Single().Value<string>();

            return new UserDetails
            {
                UserId = userId.Value,
                Email = email
            };
        }
    }
}