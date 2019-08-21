using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using LandmarkRemark.Api.Models;

namespace LandmarkRemark.Api.Middlewares
{
    /// <summary>
    /// Base class for the Middlewares used to override the response output format for any unhandled exceptions.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        protected RequestDelegate Next { get; }

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public virtual async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                await HandleException(ex, context);
            }
        }

        public virtual async Task HandleException(Exception ex, HttpContext context)
        {
            var message = "An unhandled exception has occurred.";

            var response = ApiResponseHelper.Error(message, ex.Message);

            var result = (ObjectResult) response.Result;
            context.Response.StatusCode = result.StatusCode ?? StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result.Value, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }
    }
}