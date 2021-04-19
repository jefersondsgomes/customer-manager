using CustomerManager.Models.Helpers;
using CustomerManager.Models.Helpers.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CustomerManager.Models.Entities
{
    [BsonCollection("user")]
    public class User : Document
    {
        [Required]
        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [Required]
        [BsonElement("lastName")]
        public string LastName { get; set; }

        [Required]
        [BsonElement("email")]
        public string Email { get; set; }

        [Required]
        [BsonElement("userName")]
        public string UserName { get; set; }

        [Required]
        [JsonIgnore]
        [BsonElement("password")]
        public string Password { get; set; }

        public User(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public User()
        {

        }
    }
}