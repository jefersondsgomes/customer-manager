using CustomerManager.Models.Helpers.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CustomerManager.Models.Helpers
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