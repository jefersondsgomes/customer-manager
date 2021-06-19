using MongoDB.Bson.Serialization.Attributes;

namespace CustomerManager.Models.Entities
{
    public class Phone
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("number")]
        public string Number { get; set; }
    }
}