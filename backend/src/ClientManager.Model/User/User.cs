using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClientManager.Model.User
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }        
        public string Login { get; set; }
        public string Password{ get; set; }        
    }
}