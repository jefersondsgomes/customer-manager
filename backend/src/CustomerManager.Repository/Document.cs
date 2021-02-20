using CustomerManager.Repository.Interfaces;
using MongoDB.Bson;
using System;

namespace CustomerManager.Repository
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt => Id.CreationTime;
    }
}