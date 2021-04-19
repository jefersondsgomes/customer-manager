using CustomerManager.Models.Helpers;
using CustomerManager.Models.Helpers.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CustomerManager.Models.Entities
{
    [BsonCollection("customer")]
    public class Customer : Document
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("rg")]
        public string RG { get; set; }

        [BsonElement("cpf")]
        public string CPF { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("birthDate")]
        public DateTime? BirthDate { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("phones")]
        public List<Phone> Phones { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("addresses")]
        public List<Address> Addresses { get; set; }
    }
}