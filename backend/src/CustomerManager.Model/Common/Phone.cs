using MongoDB.Bson.Serialization.Attributes;

namespace CustomerManager.Model.Common
{
    public class Phone
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("number")]
        public string Number { get; set; }
    }
}