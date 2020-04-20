using ClientManager.Repository;

namespace ClientManager.Model.User
{
    [BsonCollection("user")]
    public class User : Document
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}