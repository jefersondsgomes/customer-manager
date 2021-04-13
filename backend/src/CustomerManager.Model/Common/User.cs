using CustomerManager.Repository;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CustomerManager.Model.Common
{
    [BsonCollection("user")]
    public class User : Document
    {
        [Required]
        [BsonElement("name")]
        public string Name { get; set; }

        [Required]
        [BsonElement("login")]
        public string Login { get; set; }

        [Required]
        [BsonElement("password")]
        public string Password { get; set; }
    }
}