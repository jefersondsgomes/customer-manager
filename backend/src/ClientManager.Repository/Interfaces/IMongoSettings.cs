namespace ClientManager.Repository.Interfaces
{
    public interface IMongoSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}