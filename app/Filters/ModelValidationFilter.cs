using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using LandmarkRemark.Api.Models;

namespace LandmarkRemark.Api.Filters
{
    /// <summary>
    /// ActionFilter for overriding the response output format for request model validation.
    /// </summary>
    public class ModelValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(m => m.Value.Errors.Any())
                    .Select(m => new { m.Key, m.Value.Errors });
                context.Result = ApiResponseHelper.BadRequest("Invalid request format.", errors).Result;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}