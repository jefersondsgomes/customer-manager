using CustomerManager.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CustomerManager.Repository
{
    public abstract class Document : IDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("created")]
        public DateTime Created { get; set; }

        public Document()
        {
            Created = DateTime.Now;
        }
    }
}