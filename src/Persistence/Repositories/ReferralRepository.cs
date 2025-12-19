using Application.Interfaces.Repositories;
using Domain.Entities;
using Google.Cloud.Firestore;

namespace Persistence.Repositories
{
    public class ReferralRepository : BaseRepository<Invitation>, IReferralRepository
    {
        public ReferralRepository(FirestoreDb db) : base(db, "referrals") { }
    }
}
