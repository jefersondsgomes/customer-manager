namespace CustomerManager.Repositories.Interfaces
{
    public interface IMongoSettings
    {
        string Database { get; set; }
        string ConnectionString { get; set; }
    }
}