using ClientManager.Repository.Interfaces;

namespace ClientManager.Repository
{
    public class MongoSettings : IMongoSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}