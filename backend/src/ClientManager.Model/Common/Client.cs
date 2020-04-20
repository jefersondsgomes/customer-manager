using System;
using System.Collections.Generic;
using ClientManager.Repository;

namespace ClientManager.Model.Common
{
    [BsonCollection("client")]
    public class Client : Document
    {            
        public string Name { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Address> Addresses { get; set; }
    }
}