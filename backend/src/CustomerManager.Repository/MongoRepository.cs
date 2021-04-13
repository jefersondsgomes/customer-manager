using CustomerManager.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManager.Repository
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument>
        where TDocument : IDocument
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IMongoSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
            _collection = _database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        public async Task<TDocument> CreateAsync(TDocument t)
        {
            await _collection.InsertOneAsync(t);
            return t;
        }

        public async Task<IList<TDocument>> FindAsync()
        {
            return await _collection.Find(Builders<TDocument>.Filter.Empty).ToListAsync();
        }

        public async Task<TDocument> FindAsync(string id)
        {
            var documents = await _collection.FindAsync(x => x.Id == id);
            return await documents.FirstOrDefaultAsync();
        }

        public async Task<TDocument> FindAsync(FilterDefinition<TDocument> filter)
        {
            return await _collection.FindAsync(filter).Result.FirstOrDefaultAsync();
        }

        public async Task<TDocument> ReplaceAsync(string id, TDocument t)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, t);
            return t;
        }

        public async Task RemoveAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        private string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType
                .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                    .FirstOrDefault())?.CollectionName;
        }
    }
}