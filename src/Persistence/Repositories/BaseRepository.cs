using Application.Interfaces.Repositories;
using Domain.Contracts;
using Google.Cloud.Firestore;

namespace Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity
    {
        private readonly FirestoreDb _db;
        private readonly string _collectionName;

        protected CollectionReference Collection => _db.Collection(_collectionName);

        public BaseRepository(FirestoreDb db, string collectionName)
        {
            _db = db;
            _collectionName = collectionName;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var snapshot = await Collection.GetSnapshotAsync();
            return snapshot.Documents.Select(doc =>
            {
                var item = doc.ConvertTo<T>();
                item.Id = doc.Id;
                return item;
            }).ToList();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var doc = await Collection.Document(id).GetSnapshotAsync();
            
            if (!doc.Exists) 
                return null;

            var item = doc.ConvertTo<T>();
            item.Id = doc.Id;
            return item;
        }

        public async Task<List<T>> GetByFiltersAsync(Dictionary<string, object> filters)
        {
            Query query = _db.Collection(_collectionName);

            foreach (var filter in filters)
            {
                query = query.WhereEqualTo(filter.Key, filter.Value);
            }

            var snapshot = await query.GetSnapshotAsync();

            return snapshot.Documents.Select(doc =>
            {
                var item = doc.ConvertTo<T>();
                item.Id = doc.Id;
                return item;
            }).ToList();
        }

        public async Task AddAsync(T entity)
        {
            if (entity.Id.Equals(null))
            {
                await Collection.AddAsync(entity);
            }
            else
            {
                await Collection.Document(entity.Id).SetAsync(entity);
            }
        }

        public async Task UpdateAsync(T entity)
            => await Collection.Document(entity.Id).SetAsync(entity, SetOptions.Overwrite);

        public async Task DeleteAsync(string id)
            => await Collection.Document(id).DeleteAsync();
    }
}
