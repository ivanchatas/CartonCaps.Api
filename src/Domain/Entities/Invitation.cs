using Domain.Contracts;
using Google.Cloud.Firestore;

namespace Domain.Entities
{
    [FirestoreData]
    public class Invitation : IEntity
    {
        [FirestoreProperty]
        public string Id { get; set; }
        
        [FirestoreProperty]
        public string Channel { get; set; }

        [FirestoreProperty]
        public string RecipientName { get; set; }

        [FirestoreProperty]
        public string RecipientContact { get; set; }

        [FirestoreProperty]
        public string Status { get; set; }
        
        [FirestoreProperty]
        public string ReferrerUserId { get; set; }

        [FirestoreProperty]
        public string ReferralCode { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty]
        public DateTime? UpdatedAt { get; set; }
    }
}
