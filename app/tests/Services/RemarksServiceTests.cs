using System.Linq;
using LandmarkRemark.Api.Models;
using LandmarkRemark.Api.Repositories;
using LandmarkRemark.Api.Repositories.Models;
using LandmarkRemark.Api.Services;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace LandmarkRemark.Api.Tests.Services
{
    public class RemarksServiceTests
    {
        private readonly IRemarksRepository _repository;
        private readonly IRemarksService _service;

        public RemarksServiceTests()
        {
            _repository = Substitute.For<IRemarksRepository>();
            _service = new RemarksService(_repository);
        }

        [Fact]
        public async void GetRemarks_Should_Call_IRemarksRepository_GetRemarks()
        {
            await _service.GetRemarks();

            await _repository.Received(1).GetRemarks();
        }

        [Fact]
        public async void GetRemarks_Should_Return_Correctly()
        {
            var expected = new [] { new RemarkDetails() };
            _repository.GetRemarks().Returns(expected);

            var actual = await _service.GetRemarks();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(null, new [] {"aaabbb", "aaaccc", "bbbddd", "email"})]
        [InlineData("", new [] {"aaabbb", "aaaccc", "bbbddd", "email"})]
        [InlineData("   ", new [] {"aaabbb", "aaaccc", "bbbddd", "email"})]
        [InlineData("aaa", new [] {"aaabbb", "aaaccc"})]
        [InlineData("bbb", new [] {"aaabbb", "bbbddd"})]
        [InlineData("ccc", new [] {"aaaccc", "bbbddd"})]
        [InlineData("ddd", new [] {"aaaccc", "bbbddd"})]
        [InlineData("eee", new [] {"aaabbb", "aaaccc"})]
        [InlineData("fff", new [] {"aaabbb", "bbbddd"})]
        [InlineData("ggg", new string[] {})]
        [InlineData("111", new string[] {"aaabbb", "aaaccc", "bbbddd"})]
        [InlineData("AaA", new [] {"aaabbb", "aaaccc"})]
        public async void GetRemarks_Should_Filter_Correctly(string filter, string[] expectedEmails)
        {
            var expected = new []
            {
                new RemarkDetails { Email = "aaabbb", Remark = "eeefff111" },
                new RemarkDetails { Email = "aaaccc", Remark = "dddeee111" },
                new RemarkDetails { Email = "bbbddd", Remark = "cccfff111" },
                new RemarkDetails { Email = "email", Remark = "remark" }
            };
            _repository.GetRemarks().Returns(expected);

            var actual = await _service.GetRemarks(filter);

            var emails = actual.Select(r => r.Email);
            emails.Should().HaveSameCount(expectedEmails);
            emails.Should().BeEquivalentTo(expectedEmails);
        }

        [Fact]
        public async void AddRemark_Should_Call_IRemarksRepository_AddRemark()
        {
            var email = "email";
            var request = new AddRemarkRequest
            {
                Latitude = 123,
                Longitude = 456,
                Remark = "remarks"
            };

            RemarkDetails arg = null;
            await _repository.AddRemark(Arg.Do<RemarkDetails>(a => arg = a));

            await _service.AddRemark(email, request);

            await _repository.Received(1).AddRemark(Arg.Any<RemarkDetails>());

            arg.Email.Should().Be(email);
            arg.Latitude.Should().Be(request.Latitude);
            arg.Longitude.Should().Be(request.Longitude);
            arg.Remark.Should().Be(request.Remark);
        }

        [Fact]
        public async void AddRemark_Should_Return_Correctly()
        {
            var expected = new RemarkDetails();
            _repository.AddRemark(Arg.Any<RemarkDetails>()).Returns(expected);

            var actual = await _service.AddRemark("email", new AddRemarkRequest());

            actual.Should().Be(expected);
        }

        [Fact]
        public async void UpdateRemark_Should_Call_IRemarksRepository_UpdateRemark()
        {
            var remarkId = "remarkId";
            var request = new UpdateRemarkRequest
            {
                Remark = "remarks"
            };

            UpdatableRemarkDetails arg = null;
            await _repository.UpdateRemark(Arg.Any<string>(), Arg.Do<UpdatableRemarkDetails>(a => arg = a));

            await _service.UpdateRemark(remarkId, request);

            await _repository.Received(1).UpdateRemark(remarkId, Arg.Any<UpdatableRemarkDetails>());

            arg.Remark.Should().Be(request.Remark);
        }

        [Fact]
        public async void DeleteRemark_Should_Call_IRemarksRepository_DeleteRemark()
        {
            var remarkId = "remarkId";

            await _service.DeleteRemark(remarkId);

            await _repository.Received(1).DeleteRemark(remarkId);
        }
    }
}