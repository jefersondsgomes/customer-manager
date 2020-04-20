using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClientManager.Repository.Interfaces
{
    public interface IDocument
    {
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }
        DateTime CreatedAt { get; }
    }
}