using CustomerManager.Repository;

namespace CustomerManager.Model.Common
{
    [BsonCollection("user")]
    public class User : Document
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}