using Api.Controllers;
using Application.Dtos;
using Application.Interfaces.Services;
using Application.Wrapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Tests.Controllers
{
    public class ReferralsControllerTests
    {
        private readonly Mock<IReferralService> _serviceMock;
        private readonly ReferralsController _controller;

        public ReferralsControllerTests()
        {
            _serviceMock = new Mock<IReferralService>();
            _controller = new ReferralsController(_serviceMock.Object);
        }

        #region GetAll

        [Fact]
        public async Task GetAll_ReturnsOk_WithResult()
        {
            // Arrange
            var result = Result<List<ReferralResponse>>.Success(new List<ReferralResponse>());

            _serviceMock
                .Setup(s => s.GetAll())
                .ReturnsAsync(result);

            // Act
            var response = await _controller.GetAll();

            // Assert
            var okResult = response.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(result);

            _serviceMock.Verify(s => s.GetAll(), Times.Once);
        }

        #endregion

        #region CreateInvitation

        [Fact]
        public async Task CreateInvitation_ReturnsOk_WithResult()
        {
            // Arrange
            var request = new CreateInvitationRequest();
            var result = Result<string>.Success("Invitation link");

            _serviceMock
                .Setup(s => s.CreateInvitation(request))
                .ReturnsAsync(result);

            // Act
            var response = await _controller.CreateInvitation(request);

            // Assert
            var okResult = response.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(result);

            _serviceMock.Verify(s => s.CreateInvitation(request), Times.Once);
        }

        #endregion

        #region GetInvitations

        [Fact]
        public async Task GetInvitations_ReturnsOk_WithResult()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var result = Result<List<StatusReferralResponse>>.Success(new List<StatusReferralResponse>());

            _serviceMock
                .Setup(s => s.GetInvitations(userId))
                .ReturnsAsync(result);

            // Act
            var response = await _controller.GetInvitations(userId);

            // Assert
            var okResult = response.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(result);

            _serviceMock.Verify(s => s.GetInvitations(userId), Times.Once);
        }

        #endregion

        #region ResolveReferral

        [Fact]
        public async Task ResolveReferral_ReturnsOk()
        {
            // Arrange
            var referralId = "ABC123";

            _serviceMock
                .Setup(s => s.ResolveReferral(referralId))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _controller.ResolveReferral(referralId);

            // Assert
            response.Should().BeOfType<OkResult>();

            _serviceMock.Verify(s => s.ResolveReferral(referralId), Times.Once);
        }

        #endregion
    }

}
