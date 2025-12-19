namespace Application.Dtos
{
    public class StatusReferralResponse
    {
        /// <summary>
        /// New referral's name
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// Pending or Completed
        /// </summary>
        public string Status { get; set; }
    }
}
