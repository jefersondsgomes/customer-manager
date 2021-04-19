namespace CustomerManager.Models.Helpers.Interfaces
{
    public interface IMongoSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}