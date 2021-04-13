using CustomerManager.Repository;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CustomerManager.Model.Common
{
    [BsonCollection("customer")]
    public class Customer : Document
    {
        public string Name { get; set; }
        [BsonElement("rg")]
        public string RG { get; set; }
        [BsonElement("cpf")]
        public string CPF { get; set; }
        public int Age { get; set; }
        public DateTime? BirthDate { get; set; }
        [BsonIgnoreIfNull]
        public List<Phone> Phones { get; set; }
        [BsonIgnoreIfNull]
        public List<Address> Addresses { get; set; }
    }
}