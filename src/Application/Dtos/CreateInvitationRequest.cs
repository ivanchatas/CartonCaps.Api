namespace Application.Dtos
{
    public class CreateInvitationRequest
    {
        /// <summary>
        /// Sms or Email
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// New referral's name
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// New referral's Phone number or email
        /// </summary>
        public string RecipientContact { get; set; }

        /// <summary>
        /// Referral Code
        /// </summary>
        public string ReferralCode { get; set; }

        /// <summary>
        /// User id how send the invitation
        /// </summary>
        public string ReferrerUserId { get; set; }
    }
}
