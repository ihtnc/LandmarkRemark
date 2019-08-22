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
        public void AddRemark_Should_Call_IRemarksRepository_AddRemark()
        {
            var userId = "userId";
            var request = new AddRemarkRequest
            {
                Latitude = 123,
                Longitude = 456,
                Remark = "remarks"
            };

            RemarkDetails arg = null;
            _repository.AddRemark(Arg.Do<RemarkDetails>(a => arg = a));

            _service.AddRemark(userId, request);

            _repository.Received(1).AddRemark(Arg.Any<RemarkDetails>());

            arg.UserId.Should().Be(userId);
            arg.Latitude.Should().Be(request.Latitude);
            arg.Longitude.Should().Be(request.Longitude);
            arg.Remark.Should().Be(request.Remark);
        }

        [Fact]
        public async void AddRemark_Should_Return_Correctly()
        {
            var expected = new RemarkDetails();
            _repository.AddRemark(Arg.Any<RemarkDetails>()).Returns(expected);

            var actual = await _service.AddRemark("userId", new AddRemarkRequest());

            actual.Should().Be(expected);
        }

        [Fact]
        public void UpdateRemark_Should_Call_IRemarksRepository_UpdateRemark()
        {
            var remarkId = "remarkId";
            var request = new UpdateRemarkRequest
            {
                Remark = "remarks"
            };

            UpdatableRemarkDetails arg = null;
            _repository.UpdateRemark(Arg.Any<string>(), Arg.Do<UpdatableRemarkDetails>(a => arg = a));

            _service.UpdateRemark(remarkId, request);

            _repository.Received(1).UpdateRemark(remarkId, Arg.Any<UpdatableRemarkDetails>());

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