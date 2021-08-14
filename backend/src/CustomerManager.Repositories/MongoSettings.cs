using CustomerManager.Repositories.Interfaces;

namespace CustomerManager.Repositories
{
    public class MongoSettings : IMongoSettings
    {
        public string Database { get; set; }
        public string ConnectionString { get; set; }
    }
}