using System.Reflection;
using LandmarkRemark.Api.Models;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;

namespace LandmarkRemark.Api.Tests.Models
{
    public class SecurityRequestTests
    {
        private readonly SecurityRequest _request;

        public SecurityRequestTests()
        {
            _request = new SecurityRequest();
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
