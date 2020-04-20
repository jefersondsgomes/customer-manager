using System;
using System.Collections.Generic;

namespace ClientManager.Model.Common
{
    public class Client
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Address> Addresses { get; set; }
    }
}