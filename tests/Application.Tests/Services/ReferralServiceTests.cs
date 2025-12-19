using Application.Dtos;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Resources;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enumerables;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Application.Tests.Services 
{
    public class ReferralServiceTests
    {
        private readonly Mock<IReferralRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IOptions<ExternalUrls> _options;
        private readonly ReferralService _service;

        public ReferralServiceTests()
        {
            _repositoryMock = new Mock<IReferralRepository>();
            _mapperMock = new Mock<IMapper>();

            _options = Options.Create(new ExternalUrls
            {
                ReferralBaseUrl = "https://dl.cartoncaps.app/invite/"
            });

            _service = new ReferralService(
                _repositoryMock.Object,
                _mapperMock.Object,
                _options
            );
        }

        #region GetAll

        [Fact]
        public async Task GetAll_ReturnsMappedReferrals()
        {
            // Arrange
            var entities = new List<Invitation>
            {
                new Invitation { Id = "1" }
            };

            var mapped = new List<ReferralResponse>
            {
                new ReferralResponse()
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(entities);

            _mapperMock
                .Setup(m => m.Map<List<ReferralResponse>>(entities))
                .Returns(mapped);

            // Act
            var result = await _service.GetAll();

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Data.Should().HaveCount(1);
        }

        #endregion

        #region GenerateReferralLink

        [Fact]
        public void GenerateReferralLink_SmsChannel_ReturnsSmsMessageWithUrl()
        {
            // Act
            var result = _service.GenerateReferralLink("sms", "ABC123");

            // Assert
            result.Should().Contain("ABC123");
            result.Should().Contain("http");
        }

        [Fact]
        public void GenerateReferralLink_EmailChannel_ReturnsEmailMessageWithUrl()
        {
            // Act
            var result = _service.GenerateReferralLink("email", "ABC123");

            // Assert
            result.Should().Contain("ABC123");
            result.Should().Contain("http");
        }

        #endregion

        #region CreateInvitation

        [Fact]
        public async Task CreateInvitation_WhenReferralDoesNotExist_CreatesInvitation()
        {
            // Arrange
            var request = new CreateInvitationRequest
            {
                ReferralCode = "ABC123",
                Channel = "sms"
            };

            _repositoryMock
                .Setup(r => r.GetByFiltersAsync(It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new List<Invitation>());

            _mapperMock
                .Setup(m => m.Map<Invitation>(request))
                .Returns(new Invitation());

            // Act
            var result = await _service.CreateInvitation(request);

            // Assert
            result.Succeeded.Should().BeTrue();

            _repositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Invitation>()),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateInvitation_WhenReferralAlreadyExists_ReturnsMessage()
        {
            // Arrange
            var request = new CreateInvitationRequest
            {
                ReferralCode = "ABC123",
                Channel = "sms"
            };

            _repositoryMock
                .Setup(r => r.GetByFiltersAsync(It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new List<Invitation>
                {
                    new Invitation { ReferralCode = "ABC123" }
                });

            // Act
            var result = await _service.CreateInvitation(request);

            // Assert
            result.Messages.Should().Contain("already been sent");

            _repositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Invitation>()),
                Times.Never
            );
        }

        #endregion

        #region GetInvitations

        [Fact]
        public async Task GetInvitations_WhenInvitationsExist_ReturnsMappedResults()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            var invitations = new List<Invitation>
            {
                new Invitation { ReferrerUserId = userId }
            };

            var mapped = new List<StatusReferralResponse>
            {
                new StatusReferralResponse()
            };

            _repositoryMock
                .Setup(r => r.GetByFiltersAsync(It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(invitations);

            _mapperMock
                .Setup(m => m.Map<List<StatusReferralResponse>>(invitations))
                .Returns(mapped);

            // Act
            var result = await _service.GetInvitations(userId);

            // Assert
            result.Data.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetInvitations_WhenNoInvitations_ReturnsEmptyListWithMessage()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            _repositoryMock
                .Setup(r => r.GetByFiltersAsync(It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new List<Invitation>());

            // Act
            var result = await _service.GetInvitations(userId);

            // Assert
            result.Data.Should().BeEmpty();
            result.Messages.Should().Contain("haven't referred any friends");
        }

        #endregion

        #region ResolveReferral

        [Fact]
        public async Task ResolveReferral_WhenPending_UpdatesStatus()
        {
            // Arrange
            var invitation = new Invitation
            {
                ReferralCode = "ABC123",
                Status = StatusType.Pending.ToString()
            };

            _repositoryMock
                .Setup(r => r.GetByFiltersAsync(It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new List<Invitation> { invitation });

            // Act
            await _service.ResolveReferral("ABC123");

            // Assert
            invitation.Status.Should().Be(StatusType.Completed.ToString());

            _repositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Invitation>()),
                Times.Once
            );
        }

        [Fact]
        public async Task ResolveReferral_WhenAlreadyCompleted_ThrowsApiException()
        {
            // Arrange
            var invitation = new Invitation
            {
                ReferralCode = "ABC123",
                Status = StatusType.Completed.ToString()
            };

            _repositoryMock
                .Setup(r => r.GetByFiltersAsync(It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new List<Invitation> { invitation });

            // Act
            Func<Task> act = async () => await _service.ResolveReferral("ABC123");

            // Assert
            await act.Should().ThrowAsync<ApiException>();
        }

        [Fact]
        public async Task ResolveReferral_WhenNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByFiltersAsync(It.IsAny<Dictionary<string, object>>()))
                .ReturnsAsync(new List<Invitation>());

            // Act
            Func<Task> act = async () => await _service.ResolveReferral("NOT_FOUND");

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        #endregion
    }
}