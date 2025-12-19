using Application.Dtos;
using Application.Wrapper;

namespace Application.Interfaces.Services
{
    public interface IReferralService
    {
        /// <summary>
        /// Return al the referrals
        /// </summary>
        /// <returns></returns>
        Task<Result<List<ReferralResponse>>> GetAll();

        /// <summary>
        /// Generate the invitation message
        /// </summary>
        /// <param name="channel">Sms or Email</param>
        /// <param name="referralCode">Referral Code</param>
        /// <returns>Invitation message with url</returns>
        string GenerateReferralLink(string channel, string referralCode);

        /// <summary>
        /// Save the recipient information and create the invitation message
        /// </summary>
        /// <param name="request">Recepient and invitation details</param>
        /// <returns>Invitation message with url</returns>
        Task<Result<string>> CreateInvitation(CreateInvitationRequest request);

        /// <summary>
        /// Get referrrals by User
        /// </summary>
        /// <param name="UserId">Referral User Id</param>
        /// <returns>All the invitations by the Referrer User</returns>
        Task<Result<List<StatusReferralResponse>>> GetInvitations(string UserId);

        /// <summary>
        /// Accepted the invitation, change status from Pending to Completed
        /// </summary>
        /// <param name="referralCode">Referral Code</param>
        /// <returns>If Referral Code was completed already returns an exception message</returns>
        Task ResolveReferral(string referralCode);
    }
}
