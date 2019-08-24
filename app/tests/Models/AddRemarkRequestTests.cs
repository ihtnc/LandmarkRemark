using System.Reflection;
using LandmarkRemark.Api.Models;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;

namespace LandmarkRemark.Api.Tests.Models
{
    public class AddRemarkRequestTests
    {
        private readonly AddRemarkRequest _request;

        public AddRemarkRequestTests()
        {
            _request = new AddRemarkRequest();
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
