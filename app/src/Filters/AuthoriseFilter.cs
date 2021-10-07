using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using LandmarkRemark.Api.Models;

namespace LandmarkRemark.Api.Filters
{
    /// <summary>
    /// AuthorizationFilter for overriding the response output format for Unauthorized and Forbidden requests.
    /// </summary>
    public class AuthoriseFilter : IAsyncAuthorizationFilter
    {
        public AuthorizationPolicy Policy { get; }

        public AuthoriseFilter(AuthorizationPolicy policy)
        {
            Policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Allow Anonymous skips all authorization
            var hasAnonymousFilter = context.Filters.OfType<IAllowAnonymousFilter>().Any();
            var hasAnonymouseAttribute = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (hasAnonymousFilter || hasAnonymouseAttribute)
            {
                return;
            }

            var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
            var authenticateResult = await policyEvaluator.AuthenticateAsync(Policy, context.HttpContext);
            var authorizeResult = await policyEvaluator.AuthorizeAsync(Policy, authenticateResult, context.HttpContext, context);

            if (authorizeResult.Challenged)
            {
                context.Result = ApiResponseHelper.Unauthorised("Unauthorised.").Result;
            }
            else if (authorizeResult.Forbidden)
            {
                context.Result = ApiResponseHelper.Forbidden("Forbidden.", Policy.AuthenticationSchemes.ToArray()).Result;
            }
        }
    }
}