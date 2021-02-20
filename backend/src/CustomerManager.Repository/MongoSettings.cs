using CustomerManager.Repository.Interfaces;

namespace CustomerManager.Repository
{
    public class MongoSettings : IMongoSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}