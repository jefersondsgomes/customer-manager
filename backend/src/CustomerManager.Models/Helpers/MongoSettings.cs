using CustomerManager.Models.Helpers.Interfaces;

namespace CustomerManager.Models.Helpers
{
    public class MongoSettings : IMongoSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}