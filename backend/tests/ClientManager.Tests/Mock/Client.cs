using ClientManager.Model.Common;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace ClientManager.Test.Mock
{
    public static class Client
    {
        public static Model.Common.Client Null;
        public static Model.Common.Client Failed;
        public static Model.Common.Client Success;

        static Client()
        {
            Null = null;

            Failed = new Model.Common.Client()
            {
                Id = ObjectId.Parse("507f191e810c19729de860ea"),
                Name = "Error",
                Age = 1,
                BirthDate = DateTime.Now,
                CPF = "123",
                RG = "123",
                Addresses = null,
                Phones = null
            };

            Success = new Model.Common.Client()
            {
                Id = ObjectId.Parse("507f1f77bcf86cd799439011"),
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
