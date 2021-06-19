using CustomerManager.Models.Helpers.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerManager.Repositories.Interfaces
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        Task<IList<TDocument>> FindAsync();
        Task<TDocument> FindAsync(string id);
        Task<TDocument> FindAsync(FilterDefinition<TDocument> filter);
        Task<TDocument> ReplaceAsync(string id, TDocument t);
        Task<TDocument> CreateAsync(TDocument t);
        Task RemoveAsync(string id);
    }
}