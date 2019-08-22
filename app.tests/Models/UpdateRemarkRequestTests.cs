using System.Reflection;
using LandmarkRemark.Api.Models;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;

namespace LandmarkRemark.Api.Tests.Models
{
    public class UpdateRemarkRequestTests
    {
        private readonly UpdateRemarkRequest _request;

        public UpdateRemarkRequestTests()
        {
            _request = new UpdateRemarkRequest();
        }

        [Fact]
        public void Properties_Should_Include_JsonRequiredAttribute()
        {
            var t = _request.GetType();
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var prop in props)
            {
                prop.Should().BeDecoratedWith<JsonRequiredAttribute>();
            }
        }
    }
}
