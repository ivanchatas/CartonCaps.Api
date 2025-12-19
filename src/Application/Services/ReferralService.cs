using Application.Dtos;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Resources;
using Application.Wrapper;
using AutoMapper;
using Domain.Entities;
using Domain.Enumerables;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class ReferralService : IReferralService
    {
        private readonly IReferralRepository _repository;
        private readonly IMapper _mapper;
        private readonly ExternalUrls _settings;

        public ReferralService(IReferralRepository repository, IMapper mapper, IOptions<ExternalUrls> options)
        {
            _repository = repository;
            _mapper = mapper;
            _settings = options.Value;
        }

        public async Task<Result<List<ReferralResponse>>> GetAll()
        {
            var result = await _repository.GetAllAsync();
            var mapped = _mapper.Map<List<ReferralResponse>>(result);
            return await Result<List<ReferralResponse>>.SuccessAsync(mapped);
        }

        public string GenerateReferralLink(string channel, string referralCode)
        {
            var shareUrl = $"{_settings.ReferralBaseUrl}{referralCode}";
            var message = channel.ToLower() == ChannelType.email.ToString() ? MessagesByChannel.Email : MessagesByChannel.Sms;
            return message + $" {shareUrl}";
        }

        public async Task<Result<string>> CreateInvitation(CreateInvitationRequest request)
        {
            var referral = await GetByReferralCode(request.ReferralCode);
            var link = GenerateReferralLink(request.Channel, request.ReferralCode);
            
            if (referral == null)
            {
                var invitation = _mapper.Map<Invitation>(request);
                invitation.Id = Guid.NewGuid().ToString();
                invitation.Status = StatusType.Pending.ToString();
                invitation.CreatedAt = DateTime.UtcNow;

                await _repository.AddAsync(invitation);

                return await Result<string>.SuccessAsync(link);
            }
            else 
            {
                return await Result<string>.SuccessAsync(link, "This invitation has already been sent");
            }
        }

        public async Task<Result<List<StatusReferralResponse>>> GetInvitations(string UserId)
        { 
            var filter = new Dictionary<string, object>
            {
                { "ReferrerUserId", UserId }
            };

            var result = await _repository.GetByFiltersAsync(filter);

            if (result != null && result.Any())
            {
                var mapped = _mapper.Map<List<StatusReferralResponse>>(result);
                return await Result<List<StatusReferralResponse>>.SuccessAsync(mapped);
            }
            else 
            {
                return await Result<List<StatusReferralResponse>>.SuccessAsync(
                    new List<StatusReferralResponse>(),
                    "You haven't referred any friends yet!");
            }
        }

        public async Task ResolveReferral(string referralCode)
        {
            var referral = await GetByReferralCode(referralCode);
            if (referral != null)
            {
                if (referral.Status == StatusType.Completed.ToString())
                {
                    throw new ApiException("Referral code is no active anymore");
                }
                else
                {
                    referral.Status = StatusType.Completed.ToString();
                    referral.UpdatedAt = DateTime.UtcNow;

                    await _repository.UpdateAsync(referral);
                }
            }
            throw new KeyNotFoundException();
        }

        public async Task<Invitation?> GetByReferralCode(string referralCode) 
        {
            var filter = new Dictionary<string, object>
            {
                { "ReferralCode", referralCode }
            };

            var result = await _repository.GetByFiltersAsync(filter);

            return result.FirstOrDefault();
        }
    }
}
