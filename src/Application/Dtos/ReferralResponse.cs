namespace Application.Dtos
{
    public class ReferralResponse
    {
        public string Id { get; set; }

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
        /// Pending or Completed
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Referral Code
        /// </summary>
        public string ReferralCode { get; set; }

        /// <summary>
        /// User id how send the invitation
        /// </summary>
        public string ReferrerUserId { get; set; }

        /// <summary>
        /// Date when the invitation was send it
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date when the invitation was accepted
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
