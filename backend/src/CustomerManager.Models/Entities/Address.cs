using MongoDB.Bson.Serialization.Attributes;

namespace CustomerManager.Models.Entities
{
    public class Address
    {
        [BsonElement("street")]
        public string Street { get; set; }

        [BsonElement("number")]
        public int Number { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("zipCode")]
        public string ZipCode { get; set; }
    }
}