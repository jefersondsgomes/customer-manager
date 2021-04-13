using CustomerManager.Repository;
using System.ComponentModel.DataAnnotations;

namespace CustomerManager.Model.Common
{
    [BsonCollection("user")]
    public class User : Document
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}