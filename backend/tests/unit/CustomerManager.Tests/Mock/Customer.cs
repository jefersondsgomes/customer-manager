using CustomerManager.Model.Common;
using System;
using System.Collections.Generic;

namespace CustomerManager.Test.Mock
{
    public static class Customer
    {
        public static Model.Common.Customer Null;
        public static Model.Common.Customer Failed;
        public static Model.Common.Customer Success;

        static Customer()
        {
            Null = null;

            Failed = new Model.Common.Customer()
            {
                Id = "507f191e810c19729de860ea",
                Name = "Error",
                Age = 1,
                BirthDate = DateTime.Now,
                CPF = "123",
                RG = "123",
                Addresses = null,
                Phones = null
            };

            Success = new Model.Common.Customer()
            {
                Id = "507f1f77bcf86cd799439011",
                Name = "Success",
                Age = 2,
                BirthDate = DateTime.Now,
                CPF = "456",
                RG = "456",
                Addresses = new List<Address>(),
                Phones = new List<Phone>()
            };
        }
    }
}
